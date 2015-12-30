declare module App.Configs {
    /**
    * The RouteConfig defines the application routes, $routeProvider
    * will also be injected into the route config and will be used to
    * access the parameters.
    */
    class RouteConfig {
        static config($stateProvider: ng.ui.IStateProvider): void;
    }
}
