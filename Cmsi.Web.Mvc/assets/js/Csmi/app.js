(function () {
    angular.module('CSMI', [
        'ui.router',                    /* Routing */
        'oc.lazyLoad',                  /* ocLazyLoad */
        //'ui.bootstrap',                 /* Ui Bootstrap */
        'pascalprecht.translate',       /* Angular Translate */
        'ngIdle',                       /* Idle timer */
        'ngSanitize',                   /* ngSanitize */
        'blockUI',                       /* blockUI */
        'angularUtils.directives.dirPagination', /*Pagination*/
        'toaster',                      /* toaster*/
        'updateMeta',
        //'star-rating',
    ])


})();
