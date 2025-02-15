class Slider {
    #config = {
        transition: 300,
        autoplayDelay: 3000,
        interactionDelay: 5500,
        dragThreshold: 5,
        swipeThreshold: 0.25
    };

    #state = {
        currentSlide: 0,
        isTransitioning: false,
        isDragging: false,
        wasDragged: false,
        dragStart: null,
        startTransform: 0
    };

    #elements = {
        controller: null,
        slides: null,
        dots: null
    };

    #timers = {
        autoplay: null,
        interaction: null,
        clickPrevention: null
    };

    #preventClickHandler;

    constructor(selector = '.banner') {
        this.#initElements(selector);
        this.#initSlider();
        this.#bindEvents();
        this.#startAutoplay();
    }

    #initElements(selector) {
        this.#elements = {
            controller: document.querySelector(selector),
            slides: document.querySelectorAll(`${selector} .slide`),
            dotsContainer: document.querySelector('.dots-container')
        };
    }

    #initSlider() {
        const { controller, slides } = this.#elements;
        const slideCount = slides.length;

        controller.appendChild(slides[0].cloneNode(true));
        controller.insertBefore(slides[slideCount - 1].cloneNode(true), slides[0]);
        this.#createDots(slideCount);

        // Global click handler
        this.#preventClickHandler = (e) => {
            if (this.#state.wasDragged) {
                e.preventDefault();
                e.stopPropagation();
            }
        };

        // Add global click listener
        document.addEventListener('click', this.#preventClickHandler, true);
    }

    #createDots(count) {
        const { dotsContainer } = this.#elements;
        const dots = Array.from({ length: count }, (_, i) => {
            const dot = document.createElement('div');
            dot.className = `dot ${i === 0 ? 'active' : ''}`;
            return dot;
        });

        dotsContainer.append(...dots);
        this.#elements.dots = dots;
    }

    #updateSlider(withTransition = true) {
        const { controller } = this.#elements;
        const { transition } = this.#config;
        const { currentSlide } = this.#state;

        controller.style.transition = withTransition
            ? `transform ${transition}ms ease-out`
            : 'none';
        controller.style.transform = `translateX(-${(currentSlide + 1) * 100}%)`;

        this.#updateDots();

        if (withTransition) {
            this.#state.isTransitioning = true;
            setTimeout(() => {
                this.#state.isTransitioning = false;
                this.#normalizePosition();
            }, transition);
        }
    }

    #updateDots() {
        const { dots } = this.#elements;
        const normalizedIndex = this.#getNormalizedIndex();

        dots.forEach((dot, i) => {
            dot.classList.toggle('active', i === normalizedIndex);
        });
    }

    #getNormalizedIndex() {
        const { currentSlide } = this.#state;
        const slideCount = this.#elements.slides.length;
        return ((currentSlide % slideCount) + slideCount) % slideCount;
    }

    #normalizePosition() {
        const { currentSlide } = this.#state;
        const slideCount = this.#elements.slides.length;

        if (currentSlide < 0) {
            this.#state.currentSlide = slideCount - 1;
            this.#updateSlider(false);
        } else if (currentSlide >= slideCount) {
            this.#state.currentSlide = 0;
            this.#updateSlider(false);
        }
    }

    #handleDrag = (event) => {
        if (this.#state.isTransitioning) return;

        const isTouchEvent = event.type.includes('touch');
        const eventX = isTouchEvent ? event.touches?.[0]?.clientX : event.clientX;

        if (!eventX && !['touchend', 'mouseup', 'mouseleave'].includes(event.type)) return;

        switch (event.type) {
            case 'touchstart':
            case 'mousedown':
                this.#dragStart(eventX, event);
                break;

            case 'touchmove':
            case 'mousemove':
                this.#drag(eventX, event);
                break;

            case 'touchend':
            case 'mouseup':
            case 'mouseleave':
                this.#dragEnd(eventX);
                break;
        }
    };

    #dragStart(eventX, event) {
        this.#state.dragStart = eventX;
        this.#state.startTransform = (this.#state.currentSlide + 1) * -100;
        this.#state.wasDragged = false;
        this.#handleUserInteraction();

        // Prevent default on links at drag start
        if (event.target.closest('a')) {
            event.preventDefault();
        }
    }

    #drag(eventX, event) {
        const { dragStart, isDragging } = this.#state;
        const { dragThreshold } = this.#config;
        const { controller } = this.#elements;

        if (!dragStart) return;

        const diff = eventX - dragStart;

        if (!isDragging && Math.abs(diff) > dragThreshold) {
            this.#state.isDragging = true;
            this.#state.wasDragged = true;
            if (event.target.closest('a')) {
                event.preventDefault();
            }
        }

        if (this.#state.isDragging) {
            const newTransform = this.#state.startTransform + (diff / controller.offsetWidth * 100);
            controller.style.transition = 'none';
            controller.style.transform = `translateX(${newTransform}%)`;
        }
    }

    #dragEnd(eventX) {
        const { dragStart, isDragging } = this.#state;
        const { controller } = this.#elements;
        const { swipeThreshold } = this.#config;

        if (!dragStart) return;

        if (isDragging) {
            const diff = eventX - dragStart;
            if (Math.abs(diff) > controller.offsetWidth * swipeThreshold) {
                this.#state.currentSlide += diff > 0 ? -1 : 1;
            }
            this.#updateSlider();

            // Reset wasDragged flag after a short delay
            clearTimeout(this.#timers.clickPrevention);
            this.#timers.clickPrevention = setTimeout(() => {
                this.#state.wasDragged = false;
            }, 100);
        }

        this.#state.dragStart = null;
        this.#state.isDragging = false;
    }

    #startAutoplay() {
        const { autoplayDelay } = this.#config;

        clearInterval(this.#timers.autoplay);

        this.#timers.autoplay = setInterval(() => {
            const { isDragging, isTransitioning } = this.#state;

            if (!isDragging && !isTransitioning) {
                this.#state.currentSlide++;
                this.#updateSlider();
            }
        }, autoplayDelay);
    }

    #handleUserInteraction() {
        const { interactionDelay } = this.#config;

        clearInterval(this.#timers.autoplay);
        clearTimeout(this.#timers.interaction);

        this.#timers.interaction = setTimeout(() => {
            if (!this.#state.isDragging) {
                this.#startAutoplay();
            }
        }, interactionDelay);
    }

    #bindEvents() {
        const { controller, dotsContainer } = this.#elements;

        // Touch events
        controller.addEventListener('touchstart', this.#handleDrag, { passive: false });
        controller.addEventListener('touchmove', this.#handleDrag, { passive: false });
        controller.addEventListener('touchend', this.#handleDrag);

        // Mouse events
        controller.addEventListener('mousedown', this.#handleDrag);
        controller.addEventListener('mousemove', this.#handleDrag);
        controller.addEventListener('mouseup', this.#handleDrag);
        controller.addEventListener('mouseleave', this.#handleDrag);

        // Prevent native dragging
        controller.addEventListener('dragstart', e => e.preventDefault());

        // Dot navigation
        dotsContainer.addEventListener('click', e => {
            const dot = e.target.closest('.dot');
            if (dot && !this.#state.isTransitioning) {
                const index = this.#elements.dots.indexOf(dot);
                if (index !== -1) {
                    this.#state.currentSlide = index;
                    this.#updateSlider();
                    this.#handleUserInteraction();
                }
            }
        });
    }

    destroy() {
        const { controller } = this.#elements;

        // Remove all event listeners
        document.removeEventListener('click', this.#preventClickHandler, true);

        controller.removeEventListener('touchstart', this.#handleDrag);
        controller.removeEventListener('touchmove', this.#handleDrag);
        controller.removeEventListener('touchend', this.#handleDrag);

        controller.removeEventListener('mousedown', this.#handleDrag);
        controller.removeEventListener('mousemove', this.#handleDrag);
        controller.removeEventListener('mouseup', this.#handleDrag);
        controller.removeEventListener('mouseleave', this.#handleDrag);

        // Clear all timers
        clearInterval(this.#timers.autoplay);
        clearTimeout(this.#timers.interaction);
        clearTimeout(this.#timers.clickPrevention);
    }
}

// Initialize slider
new Slider();