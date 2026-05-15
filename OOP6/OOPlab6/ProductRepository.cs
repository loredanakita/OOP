using System;
using System.Collections.Generic;

// ─────────────────────────────────────────────────────────────────────────────
//  PATTERN: Observer (also known as Event / Publish-Subscribe)
//
//  Justification:
//    The product list is modified from multiple places (Add, Edit, Delete, Load).
//    Any component that needs to react to changes (status bar, logger, future
//    undo/redo stack) would have to be explicitly called from every place.
//    Observer decouples the list from its consumers: subscribers register once
//    and receive notifications automatically whenever the list changes.
//
//  Participants:
//    IProductObserver   — Subscriber interface
//    ProductRepository  — Publisher (Subject); also implements Singleton — see below
//    StatusBarObserver  — Concrete subscriber (updates the form's status label)
// ─────────────────────────────────────────────────────────────────────────────

namespace loriks3
{
    // ── Change descriptor ─────────────────────────────────────────────────────

    /// <summary>Describes what changed in the product list.</summary>
    public enum ProductChangeKind { Added, Removed, Replaced, Cleared }

    /// <summary>Event data passed to every observer on change.</summary>
    public sealed class ProductChangedEventArgs : EventArgs
    {
        public ProductChangeKind Kind      { get; }
        public CosmeticProduct?  Product   { get; }   // null for Cleared
        public int               Index     { get; }

        public ProductChangedEventArgs(ProductChangeKind kind,
                                       CosmeticProduct? product = null,
                                       int index = -1)
        {
            Kind    = kind;
            Product = product;
            Index   = index;
        }
    }

    // ── Observer interface ────────────────────────────────────────────────────

    /// <summary>Any object that wants to be notified of product-list changes.</summary>
    public interface IProductObserver
    {
        /// <summary>Called by the repository whenever the product list changes.</summary>
        void OnProductChanged(object sender, ProductChangedEventArgs e);
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  PATTERN: Singleton
    //
    //  Justification:
    //    ProductRepository is the single authoritative source of the product list.
    //    Multiple form components (main form, dialogs, storage layer) all need
    //    access to the same list. Singleton guarantees exactly one instance exists
    //    for the lifetime of the application and provides a global access point,
    //    eliminating the need to pass the list as a parameter everywhere.
    //
    //  Thread-safety: achieved with Lazy<T> (initialised once on first access).
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Central repository for cosmetic products.
    /// Combines Singleton (one shared instance) and Observer (change notifications).
    /// </summary>
    public sealed class ProductRepository
    {
        // ── Singleton ─────────────────────────────────────────────────────────

        private static readonly Lazy<ProductRepository> _instance =
            new(() => new ProductRepository(), isThreadSafe: true);

        /// <summary>The single application-wide repository instance.</summary>
        public static ProductRepository Instance => _instance.Value;

        /// <summary>Private constructor — prevents external instantiation.</summary>
        private ProductRepository() { }

        // ── Data ──────────────────────────────────────────────────────────────

        private readonly List<CosmeticProduct> _products = new();

        /// <summary>Read-only view of the product list.</summary>
        public IReadOnlyList<CosmeticProduct> Products => _products;

        // ── Observer: subscriber management ──────────────────────────────────

        private readonly List<IProductObserver> _observers = new();

        /// <summary>Registers an observer to receive change notifications.</summary>
        public void Subscribe(IProductObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        /// <summary>Removes a previously registered observer.</summary>
        public void Unsubscribe(IProductObserver observer) =>
            _observers.Remove(observer);

        /// <summary>Notifies all registered observers of a change.</summary>
        private void Notify(ProductChangedEventArgs args)
        {
            foreach (var obs in _observers)
                obs.OnProductChanged(this, args);
        }

        // ── Mutating operations (each triggers Observer notification) ─────────

        /// <summary>Adds a product and notifies observers.</summary>
        public void Add(CosmeticProduct product)
        {
            _products.Add(product);
            Notify(new ProductChangedEventArgs(
                ProductChangeKind.Added, product, _products.Count - 1));
        }

        /// <summary>Removes the product at the given index and notifies observers.</summary>
        public void RemoveAt(int index)
        {
            var removed = _products[index];
            _products.RemoveAt(index);
            Notify(new ProductChangedEventArgs(
                ProductChangeKind.Removed, removed, index));
        }

        /// <summary>
        /// Signals that the product at <paramref name="index"/> was edited in-place.
        /// </summary>
        public void NotifyEdited(int index)
        {
            Notify(new ProductChangedEventArgs(
                ProductChangeKind.Replaced, _products[index], index));
        }

        /// <summary>Replaces all products (e.g. after Load) and notifies observers.</summary>
        public void ReplaceAll(IEnumerable<CosmeticProduct> products)
        {
            _products.Clear();
            _products.AddRange(products);
            Notify(new ProductChangedEventArgs(ProductChangeKind.Cleared));
        }

        /// <summary>Removes all products and notifies observers.</summary>
        public void Clear()
        {
            _products.Clear();
            Notify(new ProductChangedEventArgs(ProductChangeKind.Cleared));
        }
    }

    // ── Concrete Observer: updates the form's status label ────────────────────

    /// <summary>
    /// Concrete Observer: listens to ProductRepository changes
    /// and writes a human-readable summary to a Label control.
    /// </summary>
    public sealed class StatusBarObserver : IProductObserver
    {
        private readonly System.Windows.Forms.Label _label;

        public StatusBarObserver(System.Windows.Forms.Label label) => _label = label;

        /// <inheritdoc/>
        public void OnProductChanged(object sender, ProductChangedEventArgs e)
        {
            string msg = e.Kind switch
            {
                ProductChangeKind.Added    => $"Added: {e.Product}",
                ProductChangeKind.Removed  => $"Deleted: {e.Product}",
                ProductChangeKind.Replaced => $"Edited: {e.Product}",
                ProductChangeKind.Cleared  => "Product list updated.",
                _                          => "Product list changed."
            };
            _label.Text = msg;
        }
    }
}
