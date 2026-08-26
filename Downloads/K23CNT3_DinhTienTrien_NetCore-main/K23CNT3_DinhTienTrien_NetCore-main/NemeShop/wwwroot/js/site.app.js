// wwwroot/js/site.app.js - Modern Enhanced Version
(function () {
    'use strict';

    // Modern utility functions
    const $ = selector => document.querySelector(selector);
    const $$ = selector => Array.from(document.querySelectorAll(selector));
    const clamp = (num, min, max) => Math.min(Math.max(num, min), max);

    // Modern currency formatter
    const formatCurrency = (amount, currency = 'VND') => {
        return new Intl.NumberFormat('vi-VN', {
            style: 'currency',
            currency: currency
        }).format(amount);
    };

    // Advanced debounce with leading/trailing options
    const debounce = (func, wait, immediate = false) => {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                timeout = null;
                if (!immediate) func(...args);
            };
            const callNow = immediate && !timeout;
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
            if (callNow) func(...args);
        };
    };

    // Smooth scroll to element
    const smoothScrollTo = (element, duration = 500) => {
        const target = typeof element === 'string' ? $(element) : element;
        if (!target) return;

        const targetPosition = target.getBoundingClientRect().top + window.pageYOffset;
        const startPosition = window.pageYOffset;
        const distance = targetPosition - startPosition;
        let startTime = null;

        function animation(currentTime) {
            if (startTime === null) startTime = currentTime;
            const timeElapsed = currentTime - startTime;
            const run = easeInOutQuad(timeElapsed, startPosition, distance, duration);
            window.scrollTo(0, run);
            if (timeElapsed < duration) requestAnimationFrame(animation);
        }

        function easeInOutQuad(t, b, c, d) {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        requestAnimationFrame(animation);
    };

    // Modern image lazy loading with intersection observer
    const initLazyLoading = () => {
        if ('IntersectionObserver' in window) {
            const lazyImageObserver = new IntersectionObserver((entries, observer) => {
                entries.forEach(entry => {
                    if (entry.isIntersecting) {
                        const lazyImage = entry.target;
                        lazyImage.src = lazyImage.dataset.src;
                        lazyImage.classList.remove('lazy');
                        lazyImageObserver.unobserve(lazyImage);
                    }
                });
            });

            $$('img[data-src]').forEach(lazyImage => {
                lazyImageObserver.observe(lazyImage);
            });
        }
    };

    // Enhanced toast notification system
    const Toast = {
        show(message, type = 'info', duration = 3000) {
            const toast = document.createElement('div');
            toast.className = `modern-toast toast-${type}`;
            toast.innerHTML = `
                <div class="toast-content">
                    <i class="toast-icon bi bi-${this.getIcon(type)}"></i>
                    <span class="toast-message">${message}</span>
                </div>
                <button class="toast-close" onclick="this.parentElement.remove()">
                    <i class="bi bi-x"></i>
                </button>
            `;

            const container = $('.toast-container') || this.createContainer();
            container.appendChild(toast);

            // Animate in
            setTimeout(() => toast.classList.add('show'), 10);

            // Auto remove
            if (duration > 0) {
                setTimeout(() => {
                    toast.classList.remove('show');
                    setTimeout(() => toast.remove(), 300);
                }, duration);
            }

            return toast;
        },

        getIcon(type) {
            const icons = {
                success: 'check-circle',
                error: 'exclamation-circle',
                warning: 'exclamation-triangle',
                info: 'info-circle'
            };
            return icons[type] || 'info-circle';
        },

        createContainer() {
            const container = document.createElement('div');
            container.className = 'toast-container';
            document.body.appendChild(container);
            return container;
        }
    };

    // Modern Shop Page with enhanced features
    window.ShopPage = {
        config: {
            productGridSelector: '#productGrid',
            hubUrl: '/realtime-hub',
            itemsPerPage: 12,
            animationDuration: 300
        },

        init(opts = {}) {
            this.config = { ...this.config, ...opts };
            this.grid = $(this.config.productGridSelector);
            this.bindEvents();
            this.initFilters();
            this.connectRealtime();

            console.log('🛍️ ShopPage initialized');
        },

        bindEvents() {
            // Enhanced search with debounce
            const searchInput = $('#searchInput');
            if (searchInput) {
                searchInput.addEventListener('input', debounce(() => {
                    this.performSearch();
                }, 400));
            }

            // Add to cart with animation
            document.addEventListener('click', (e) => {
                if (e.target.closest('.add-to-cart')) {
                    const btn = e.target.closest('.add-to-cart');
                    const id = parseInt(btn.dataset.id);
                    const qty = parseInt(btn.dataset.quantity) || 1;
                    this.addToCartWithAnimation(btn, id, qty);
                }
            });

            // Wishlist toggle
            document.addEventListener('click', (e) => {
                if (e.target.closest('.wishlist-toggle')) {
                    const btn = e.target.closest('.wishlist-toggle');
                    const productId = parseInt(btn.dataset.productId);
                    this.toggleWishlist(btn, productId);
                }
            });

            // Quick view
            document.addEventListener('click', (e) => {
                if (e.target.closest('.quick-view-btn')) {
                    const btn = e.target.closest('.quick-view-btn');
                    const productId = parseInt(btn.dataset.productId);
                    this.showQuickView(productId);
                }
            });
        },

        initFilters() {
            const filterElements = $$('#categorySelect, #sortSelect, #priceMin, #priceMax, #brandSelect');
            filterElements.forEach(filter => {
                filter.addEventListener('change', debounce(() => {
                    this.performSearch();
                }, 300));
            });
        },

        async performSearch(page = 1) {
            try {
                this.showLoading();

                const params = new URLSearchParams({
                    q: $('#searchInput')?.value || '',
                    cat: $('#categorySelect')?.value || '',
                    min: $('#priceMin')?.value || '',
                    max: $('#priceMax')?.value || '',
                    sort: $('#sortSelect')?.value || '',
                    brand: $('#brandSelect')?.value || '',
                    page: page.toString()
                });

                const response = await fetch(`/Home/SearchProducts?${params}`);
                if (!response.ok) throw new Error('Search request failed');

                const html = await response.text();
                await this.updateProductGrid(html);

            } catch (error) {
                console.error('Search error:', error);
                Toast.show('Lỗi tải sản phẩm', 'error');
            } finally {
                this.hideLoading();
            }
        },

        async updateProductGrid(html) {
            if (!this.grid) return;

            // Add fade out animation
            this.grid.style.opacity = '0';

            await new Promise(resolve => setTimeout(resolve, this.config.animationDuration));

            this.grid.innerHTML = html;

            // Add fade in animation
            this.grid.style.opacity = '1';

            // Re-initialize lazy loading
            initLazyLoading();

            // Refresh AOS animations
            if (window.AOS) AOS.refresh();
        },

        addToCartWithAnimation(button, productId, quantity = 1) {
            const originalText = button.innerHTML;
            const originalState = button.disabled;

            // Show loading state
            button.innerHTML = '<i class="bi bi-arrow-repeat spinner-border spinner-border-sm"></i>';
            button.disabled = true;

            // Perform add to cart
            CartPage.addToCart(productId, quantity);

            // Success animation
            button.innerHTML = '<i class="bi bi-check-lg"></i> Đã thêm';
            button.classList.add('btn-success');

            Toast.show('Đã thêm vào giỏ hàng', 'success');

            // Reset button after animation
            setTimeout(() => {
                button.innerHTML = originalText;
                button.disabled = originalState;
                button.classList.remove('btn-success');
            }, 2000);
        },

        async toggleWishlist(button, productId) {
            try {
                const isActive = button.classList.contains('active');
                const token = $('input[name="__RequestVerificationToken"]')?.value;

                const response = await fetch('/Wishlist/ToggleWishlist', {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': token,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ productId })
                });

                const result = await response.json();

                if (result.success) {
                    button.classList.toggle('active', result.isWishlist);
                    button.querySelector('i').className = result.isWishlist ?
                        'bi bi-heart-fill text-danger' : 'bi bi-heart';

                    Toast.show(result.message, 'success');
                } else {
                    Toast.show(result.message, 'error');
                }
            } catch (error) {
                console.error('Wishlist error:', error);
                Toast.show('Lỗi cập nhật yêu thích', 'error');
            }
        },

        async showQuickView(productId) {
            try {
                const response = await fetch(`/Home/QuickView/${productId}`);
                const html = await response.text();

                // Create modal container
                let modal = $('#quickViewModal');
                if (!modal) {
                    modal = document.createElement('div');
                    modal.id = 'quickViewModal';
                    modal.className = 'modal fade';
                    modal.innerHTML = `
                        <div class="modal-dialog modal-lg modal-dialog-centered">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title">Xem nhanh sản phẩm</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                </div>
                                <div class="modal-body" id="quickViewContent">
                                </div>
                            </div>
                        </div>
                    `;
                    document.body.appendChild(modal);
                }

                $('#quickViewContent').innerHTML = html;
                const bootstrapModal = new bootstrap.Modal(modal);
                bootstrapModal.show();

            } catch (error) {
                console.error('Quick view error:', error);
                Toast.show('Lỗi tải thông tin sản phẩm', 'error');
            }
        },

        showLoading() {
            // Show loading overlay
            let loader = $('.page-loader');
            if (!loader) {
                loader = document.createElement('div');
                loader.className = 'page-loader';
                loader.innerHTML = '<div class="loader-spinner"></div>';
                document.body.appendChild(loader);
            }
            loader.style.display = 'flex';
        },

        hideLoading() {
            const loader = $('.page-loader');
            if (loader) {
                loader.style.display = 'none';
            }
        },

        connectRealtime() {
            if (!this.config.hubUrl || typeof signalR === 'undefined') return;

            const connection = new signalR.HubConnectionBuilder()
                .withUrl(this.config.hubUrl)
                .withAutomaticReconnect()
                .build();

            connection.on('ProductUpdated', (productId) => {
                const productCard = $(`.product-card[data-product-id="${productId}"]`);
                if (productCard) {
                    productCard.classList.add('highlight-update');
                    setTimeout(() => productCard.classList.remove('highlight-update'), 2000);
                }
            });

            connection.on('ReviewAdded', (productId, reviewId) => {
                // Handle new review
                console.log('New review added:', reviewId);
            });

            connection.start().catch(err => console.error('SignalR connection error:', err));
        }
    };

    // Enhanced Cart System
    window.CartPage = {
        storageKey: 'neme_cart_v2',
        config: {
            animationDuration: 500,
            maxQuantity: 10
        },

        getCart() {
            try {
                return JSON.parse(localStorage.getItem(this.storageKey) || '[]');
            } catch {
                return [];
            }
        },

        setCart(cart) {
            localStorage.setItem(this.storageKey, JSON.stringify(cart));
            this.updateCartUI();
        },

        addToCart(productId, quantity = 1) {
            const cart = this.getCart();
            const existingItem = cart.find(item => item.id === productId);

            if (existingItem) {
                existingItem.quantity = clamp(existingItem.quantity + quantity, 1, this.config.maxQuantity);
            } else {
                cart.push({
                    id: productId,
                    quantity: clamp(quantity, 1, this.config.maxQuantity),
                    addedAt: new Date().toISOString()
                });
            }

            this.setCart(cart);
            this.triggerCartAnimation();
        },

        removeFromCart(productId) {
            const cart = this.getCart().filter(item => item.id !== productId);
            this.setCart(cart);
        },

        updateQuantity(productId, newQuantity) {
            const cart = this.getCart();
            const item = cart.find(item => item.id === productId);

            if (item) {
                item.quantity = clamp(newQuantity, 1, this.config.maxQuantity);
                this.setCart(cart);
            }
        },

        getTotalItems() {
            return this.getCart().reduce((total, item) => total + item.quantity, 0);
        },

        async updateCartUI() {
            // Update cart count in navbar
            const cartCountElements = $$('.cart-count, .cart-badge');
            const totalItems = this.getTotalItems();

            cartCountElements.forEach(element => {
                element.textContent = totalItems;
                element.style.display = totalItems > 0 ? 'flex' : 'none';
            });

            // Update cart sidebar if open
            const cartSidebar = $('#cartSidebar');
            if (cartSidebar && new bootstrap.Offcanvas(cartSidebar)._isShown) {
                await this.renderCartContents();
            }
        },

        async renderCartContents() {
            const container = $('#cartContents');
            if (!container) return;

            const cart = this.getCart();
            if (cart.length === 0) {
                container.innerHTML = this.getEmptyCartHTML();
                return;
            }

            try {
                const productIds = cart.map(item => item.id).join(',');
                const response = await fetch(`/SanPhams/GetByIds?ids=${productIds}`);

                if (response.ok) {
                    const products = await response.json();
                    container.innerHTML = this.generateCartHTML(cart, products);
                    this.bindCartEvents();
                }
            } catch (error) {
                console.error('Cart render error:', error);
                container.innerHTML = '<div class="text-danger">Lỗi tải giỏ hàng</div>';
            }
        },

        generateCartHTML(cart, products) {
            return cart.map(cartItem => {
                const product = products.find(p => p.maSp === cartItem.id);
                if (!product) return '';

                const subtotal = product.donGia * cartItem.quantity;

                return `
                    <div class="cart-item" data-product-id="${product.maSp}">
                        <div class="cart-item-image">
                            <img src="${product.hinhAnh || '/images/placeholder.png'}" 
                                 alt="${product.tenSp}"
                                 onerror="this.src='/images/placeholder.png'">
                        </div>
                        <div class="cart-item-details">
                            <h6 class="cart-item-title">${product.tenSp}</h6>
                            <div class="cart-item-price">${formatCurrency(product.donGia)}</div>
                            <div class="cart-item-actions">
                                <div class="quantity-controls">
                                    <button class="btn btn-sm btn-outline-secondary quantity-decrease" 
                                            data-product-id="${product.maSp}">
                                        <i class="bi bi-dash"></i>
                                    </button>
                                    <span class="quantity-display">${cartItem.quantity}</span>
                                    <button class="btn btn-sm btn-outline-secondary quantity-increase" 
                                            data-product-id="${product.maSp}">
                                        <i class="bi bi-plus"></i>
                                    </button>
                                </div>
                                <button class="btn btn-sm btn-outline-danger remove-item" 
                                        data-product-id="${product.maSp}">
                                    <i class="bi bi-trash"></i>
                                </button>
                            </div>
                        </div>
                        <div class="cart-item-subtotal">
                            ${formatCurrency(subtotal)}
                        </div>
                    </div>
                `;
            }).join('');
        },

        bindCartEvents() {
            // Quantity controls
            $$('.quantity-increase').forEach(btn => {
                btn.addEventListener('click', () => {
                    const productId = parseInt(btn.dataset.productId);
                    const cart = this.getCart();
                    const item = cart.find(item => item.id === productId);
                    if (item) this.updateQuantity(productId, item.quantity + 1);
                });
            });

            $$('.quantity-decrease').forEach(btn => {
                btn.addEventListener('click', () => {
                    const productId = parseInt(btn.dataset.productId);
                    const cart = this.getCart();
                    const item = cart.find(item => item.id === productId);
                    if (item && item.quantity > 1) {
                        this.updateQuantity(productId, item.quantity - 1);
                    }
                });
            });

            // Remove items
            $$('.remove-item').forEach(btn => {
                btn.addEventListener('click', () => {
                    const productId = parseInt(btn.dataset.productId);
                    this.removeFromCart(productId);
                });
            });
        },

        getEmptyCartHTML() {
            return `
                <div class="empty-cart text-center py-5">
                    <i class="bi bi-cart-x display-1 text-muted"></i>
                    <h5 class="mt-3 text-muted">Giỏ hàng trống</h5>
                    <p class="text-muted">Hãy thêm sản phẩm vào giỏ hàng</p>
                    <a href="/Home" class="btn btn-primary mt-3">
                        <i class="bi bi-arrow-left me-2"></i>Tiếp tục mua sắm
                    </a>
                </div>
            `;
        },

        triggerCartAnimation() {
            // Add bounce animation to cart icon
            const cartIcons = $$('.cart-icon, .mobile-cart-icon');
            cartIcons.forEach(icon => {
                icon.style.transform = 'scale(1.2)';
                setTimeout(() => {
                    icon.style.transform = 'scale(1)';
                }, 300);
            });
        }
    };

    // Initialize everything when DOM is ready
    document.addEventListener('DOMContentLoaded', function () {
        // Initialize core systems
        initLazyLoading();

        // Initialize ShopPage if available
        if (window.ShopPage) {
            ShopPage.init({
                productGridSelector: '#productGrid',
                hubUrl: '/realtime-hub'
            });
        }

        // Initialize CartPage
        if (window.CartPage) {
            CartPage.updateCartUI();
        }

        // Add modern toast styles
        const toastStyles = document.createElement('style');
        toastStyles.textContent = `
            .toast-container {
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 1060;
                max-width: 400px;
            }
            
            .modern-toast {
                background: var(--surface-color);
                border: 1px solid var(--gray-200);
                border-radius: 12px;
                padding: 1rem;
                margin-bottom: 1rem;
                box-shadow: var(--shadow-lg);
                opacity: 0;
                transform: translateX(100%);
                transition: all 0.3s ease;
                display: flex;
                align-items: center;
                justify-content: between;
            }
            
            .modern-toast.show {
                opacity: 1;
                transform: translateX(0);
            }
            
            .toast-content {
                display: flex;
                align-items: center;
                gap: 0.75rem;
                flex: 1;
            }
            
            .toast-icon {
                font-size: 1.25rem;
            }
            
            .toast-success .toast-icon { color: var(--success-color); }
            .toast-error .toast-icon { color: var(--danger-color); }
            .toast-warning .toast-icon { color: var(--warning-color); }
            .toast-info .toast-icon { color: var(--primary-color); }
            
            .toast-message {
                font-weight: 500;
                color: var(--text-color);
            }
            
            .toast-close {
                background: none;
                border: none;
                color: var(--text-muted);
                padding: 0.25rem;
                border-radius: 4px;
                transition: all 0.2s ease;
            }
            
            .toast-close:hover {
                background: var(--gray-200);
                color: var(--text-color);
            }
        `;
        document.head.appendChild(toastStyles);

        console.log('🚀 Modern eCommerce System Initialized');
    });

    // Global error handler
    window.addEventListener('error', function (e) {
        console.error('Global error:', e.error);
        Toast.show('Đã xảy ra lỗi không mong muốn', 'error');
    });

})();