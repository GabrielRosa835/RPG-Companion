### Services Lifetimes
> Where do I need a scope?
> * In situations with temporary singletons
> * During event processing -> may not be temporary
> * Make it temporary: reset when no event left or after a set timer
