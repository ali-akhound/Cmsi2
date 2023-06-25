/**
 * INSPINIA - Responsive Admin Theme
 *
  Other libraries are loaded dynamically in the config.js file using the library ocLazyLoad
 */
(function () {
    angular.module('inspinia', [
        'ui.router',                    /* Routing */
        'oc.lazyLoad',                  /* ocLazyLoad */
        'ui.bootstrap',                 /* Ui Bootstrap */
        'pascalprecht.translate',       /* Angular Translate */
        'ngIdle',                       /* Idle timer */
        'ngSanitize',                   /* ngSanitize */
        'blockUI',                       /* blockUI */
        'toaster',                      /* toaster*/
    ])


})();
