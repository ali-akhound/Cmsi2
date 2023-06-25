function config($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, IdleProvider, KeepaliveProvider, blockUIConfig, $locationProvider) {
    /* Change the default overlay message /*/
    blockUIConfig.message = '';
    /* Change the default delay to 100ms before the blocking is visible /*/
    blockUIConfig.delay = 100;
    blockUIConfig.template =
        '<div class="block-ui-overlay"></div>' +
        '<div class="block-ui-message-container">' +
        '<div class="sk-spinner sk-spinner-double-bounce">' +
        '<div class="sk-double-bounce1"></div>' +
        '<div class="sk-double-bounce2"></div>' +
        '</div>' +
        '</div>';
    /*blockUIConfig.template = '<div class="block-ui-message-container"><div class="sk-spinner sk-spinner-double-bounce"><div class="sk-double-bounce1"></div><div class="sk-double-bounce2"></div></div></div>';/*/

    /* Configure Idle settings /*/
    IdleProvider.idle(5); /* in seconds /*/
    IdleProvider.timeout(120); /* in seconds /*/
    $urlRouterProvider.otherwise(function ($injector, $location) {
        var state = $injector.get('$state');
        state.go("Home", { Lang: getCookie('Lang') }); // here we get { query: ... }
        return $location.path();
    });
    //$urlRouterProvider.otherwise("Home");

    $ocLazyLoadProvider.config({
        // Set to true if you want to see what and when is dynamically loaded
        debug: false
    });

    $stateProvider
        .state('Home', {
            url: "/UI/Home?Lang",
            title: 'Home',
            templateUrl: "Home/_Home"
        })
        .state('ContactUs', {
            url: "/UI/ContactUs?Lang",
            title: 'ContactUs',
            templateUrl: "Home/ContactUs"
        })
        .state('Signup', {
            url: "/UI/Signup?Lang",
            title: 'Signup',
            templateUrl: "Home/Signup"
        })

        /**************************** DynamicPage ****************************/
        .state('DynamicPage', {
            url: "/UI/DynamicPage?Lang&id",
            title: ' ',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/DynamicPage?id=" + stateParams.id;
            }
        })

        /**************************** ResultBank ****************************/
        .state('ResultBank', {
            cache: false,
            url: "/UI/ResultBank?Lang&id&tbl",
            title: 'ResultBank',
            templateUrl: function (stateParams) {
                return "/Home/ResultBank?id=" + stateParams.id + "&tbl=" + stateParams.tbl;
            }
        })
        /**************************** Membership ****************************/
        .state('Login', {
            cache: false,
            url: "/UI/Login?Lang",
            title: 'Login',
            templateUrl: '/Home/Login'
        })
        .state('ConfirmEmail', {
            cache: false,
            url: "/UI/ConfirmEmail?Lang&userId&code",
            title: 'ConfirmEmail',
            templateUrl: function (stateParams) {
                return "/Home/ConfirmEmail?userId=" + stateParams.userId + "&code=" + stateParams.code;
            }
        })
        .state('PasswordRecovery', {
            cache: false,
            url: "/UI/PasswordRecovery?Lang",
            title: 'PasswordRecovery',
            templateUrl: '/Home/PasswordRecovery'
        })
        .state('ResetPassword', {
            cache: false,
            url: "/UI/ResetPassword?Lang&id&code",
            title: 'ResetPassword',
            templateUrl: function (stateParams) {
                return "/Home/ResetPassword?id=" + stateParams.id + "&code=" + stateParams.code;
            }
        })
        .state('Error', {
            cache: false,
            url: "/UI/Error?Lang",
            title: 'Error',
            templateUrl: '/Home/Error'
        })
        /**************************** User Profile ****************************/
        .state('UserProfile', {
            cache: false,
            url: "/UI/ProfileIndex?Lang",
            title: 'ProfileIndex',
            templateUrl: '/Home/ProfileIndex'
        })
        .state('UserProfile.Home', {
            url: "/UI/HomeProfile?Lang",
            title: 'HomeProfile',
            templateUrl: '/Home/HomeProfile'
        })
        .state('UserProfile.Profile', {
            url: "/UI/Profile?Lang",
            title: 'Profile',
            templateUrl: '/Home/ProfileForm'
        })
        .state('UserProfile.ProfileChangePass', {
            url: "/UI/ChangePass?Lang",
            title: 'ChangePass',
            templateUrl: '/Home/ChangePassAction'
        })
        .state('UserProfile.ArticleList', {
            url: "/UI/ArticleList?Lang&pageNumber",
            title: 'ArticleList',
            templateUrl: function (stateParams) {
                return "/Home/ArticleList?pageNumber=" + stateParams.pageNumber;
            }
        })
        .state('UserProfile.TemplateArticle', {
            url: "/UI/TemplateArticle?Lang",
            title: 'TemplateArticle',
            templateUrl: '/Home/TemplateArticle'
        })
        .state('UserProfile.Article', {
            url: "/UI/Article?Lang&id",
            title: 'Article',
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: [
                                '/assets/css/wizard/smart_wizard_theme_arrows.css',
                                '/assets/css/wizard/smart_wizard.css',
                                '/assets/js/wizard/jquery.smartWizard.min.js'
                            ]
                        }
                    ]);
                }
            },
            templateUrl: function (stateParams) {
                return "/Home/Article?id=" + stateParams.id;
            }
        })
        .state('UserProfile.Vote', {
            cache: false,
            url: "/Vote",
            title: 'Vote',
            templateUrl: function (stateParams) {
                return "/ClientVote/Vote";
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.switchery',
                            files: ['/Areas/Admin/assets/css/plugins/switchery/switchery.css', '/Areas/Admin/assets/js/plugins/switchery/switchery.js', '/Areas/Admin/assets/js/plugins/switchery/ng-switchery.js']
                        }
                    ]);
                }
            }
        })
        .state('UserProfile.RegisterCompanion', {
            url: "/UI/RegisterCompanion?Lang",
            title: 'RegisterCompanion',
            templateUrl: '/Home/RegisterCompanion'
        })
        .state('UserProfile.CompanionList', {
            url: "/UI/CompanionList?Lang&pageNumber",
            title: 'CompanionList',
            templateUrl: function (stateParams) {
                return "/Home/CompanionList?pageNumber=" + stateParams.pageNumber;
            }
        })
        .state('UserProfile.OrderList', {
            url: "/UI/OrderList?Lang&pageNumber",
            title: 'OrderList',
            templateUrl: function (stateParams) {
                return "/Home/OrderList?pageNumber=" + stateParams.pageNumber;
            }
        })
        .state('UserProfile.PayConference', {
            url: "/UI/PayConference?Lang&OrderID",
            title: 'PayConference',
            templateUrl: function (stateParams) {
                return "/Home/PayConference?OrderID=" + stateParams.OrderID;
            }
        })
        .state('UserProfile.PayVip', {
            url: "/UI/PayVip?Lang&OrderID",
            title: 'PayVip',
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: [
                                '/assets/css/wizard/smart_wizard_theme_arrows.css',
                                '/assets/css/wizard/smart_wizard.css',
                                '/assets/js/wizard/jquery.smartWizard.min.js'
                            ]
                        }
                    ]);
                }
            },
            templateUrl: function (stateParams) {
                return "/Home/PayVip?OrderID=" + stateParams.OrderID;
            }
        })
        .state('UserProfile.Pay', {
            url: "/UI/Pay?Lang&OrderID&Type",
            title: 'Pay',
            templateUrl: function (stateParams) {
                return "/Home/Pay?OrderID=" + stateParams.OrderID + "&Type=" + stateParams.Type;
            }
        })


        /**************************** AnnualProgram ****************************/
        .state('AnnualProgram', {
            cache: false,
            url: "/UI/AnnualProgram?Lang",
            title: 'AnnualProgram',
            templateUrl: '/Csmi/AnnualProgram'
        })
        /**************************** ArticleWrite ****************************/
        .state('ArticleWrite', {
            cache: false,
            url: "/UI/ArticleWrite?Lang",
            title: 'ArticleWrite',
            templateUrl: '/Csmi/ArticleWrite'
        })

        /**************************** BookList ****************************/
        .state('BookList', {
            cache: false,
            url: "/UI/BookList?Lang&pageNumber&WriterName&OrderType",
            title: 'BookList',
            templateUrl: function (stateParams) {
                return '/Csmi/BookList?pageNumber=' + stateParams.pageNumber + "&WriterName=" + stateParams.WriterName + '&OrderType=' + stateParams.OrderType;
            }
        })

        /**************************** FAQ ****************************/
        .state('FAQ', {
            cache: false,
            url: "/UI/FAQ?Lang&pageNumber",
            title: 'FAQ',
            templateUrl: function (stateParams) {
                return "/Csmi/FAQ?pageNumber=" + stateParams.pageNumber;
            }
        })
        /**************************** Gallary ****************************/
        .state('Gallary', {
            cache: false,
            url: "/UI/Gallary?Lang",
            title: 'Gallary',
            templateUrl: '/Csmi/Gallary'
        })
        .state('GallaryInfo', {
            cache: false,
            url: "/UI/GallaryInfo?Lang&AlbumID",
            title: 'GallaryInfo',
            templateUrl: function (stateParams) {
                return '/Csmi/GallaryInfo?AlbumID=' + stateParams.AlbumID;
            }
        })

        /**************************** HistoryCrystal ****************************/
        .state('HistoryCrystal', {
            cache: false,
            url: "/UI/HistoryCrystal?Lang",
            title: 'HistoryCrystal',
            templateUrl: '/Csmi/HistoryCrystal'
        })
        /**************************** MenuMajalehBases ****************************/
        .state('MajalehBases', {
            cache: false,
            url: "/UI/MajalehBases?Lang",
            title: 'MajalehBases',
            templateUrl: '/Csmi/MajalehBases'
        })
        /**************************** MajalehMember ****************************/
        .state('MajalehMember', {
            cache: false,
            url: "/UI/MajalehMember?Lang",
            title: 'MajalehMember',
            templateUrl: '/Csmi/MajalehMember'
        })
        /**************************** MajalehAbout ****************************/
        .state('MajalehAbout', {
            cache: false,
            url: "/UI/MajalehAbout?Lang",
            title: 'MajalehAbout',
            templateUrl: '/Csmi/MajalehAbout'
        })

        /**************************** GuideArticleSend ****************************/
        .state('GuideArticleSend', {
            cache: false,
            url: "/UI/GuideArticleSend?Lang",
            title: 'GuideArticleSend',
            templateUrl: '/Csmi/GuideArticleSend'
        })

        /**************************** GuideArticleArbiter ****************************/
        .state('GuideArticleArbiter', {
            cache: false,
            url: "/UI/GuideArticleArbiter?Lang",
            title: 'GuideArticleArbiter',
            templateUrl: '/Csmi/GuideArticleArbiter'
        })
        /**************************** GuideSitemap ****************************/
        .state('GuideSitemap', {
            cache: false,
            url: "/UI/GuideSitemap?Lang",
            title: 'GuideSitemap',
            templateUrl: '/Csmi/GuideSitemap'
        })
        /**************************** SocietyBases ****************************/
        .state('SocietyBases', {
            cache: false,
            url: "/UI/SocietyBases?Lang",
            title: 'SocietyBases',
            templateUrl: '/Csmi/SocietyBases'
        })
        /**************************** SocietyCertificate ****************************/
        .state('SocietyCertificate', {
            cache: false,
            url: "/UI/SocietyCertificate?Lang",
            title: 'SocietyCertificate',
            templateUrl: '/Csmi/SocietyCertificate'
        })
        /**************************** SocietyChart ****************************/
        .state('SocietyChart', {
            cache: false,
            url: "/UI/SocietyChart?Lang",
            title: 'SocietyChart',
            templateUrl: '/Csmi/SocietyChart',
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['/assets/css/SocietyChart.css']
                        }
                    ]);
                }
            }
        })
        /**************************** SocietyDirector ****************************/
        .state('SocietyDirector', {
            cache: false,
            url: "/UI/SocietyDirector?Lang",
            title: 'SocietyDirector',
            templateUrl: '/Csmi/SocietyDirector'
        })
        /**************************** SocietyHistory ****************************/
        .state('SocietyHistory', {
            cache: false,
            url: "/UI/SocietyHistory?Lang",
            title: 'SocietyHistory',
            templateUrl: '/Csmi/SocietyHistory'
        })
        /**************************** SocietyHistory ****************************/
        .state('SocietyMembers', {
            cache: false,
            url: "/UI/SocietyMembers?Lang",
            title: 'SocietyMembers',
            templateUrl: '/Csmi/SocietyMembers'
        })
        /**************************** SocietyActivities ****************************/
        .state('SocietyActivities', {
            cache: false,
            url: "/UI/SocietyActivities?Lang",
            title: 'SocietyActivities',
            templateUrl: '/Csmi/SocietyActivities'
        })
        /**************************** SocietyGoals ****************************/
        .state('SocietyGoals', {
            cache: false,
            url: "/UI/SocietyGoals?Lang",
            title: 'SocietyGoals',
            templateUrl: '/Csmi/SocietyGoals'
        })
        /**************************** SocientyRules ****************************/
        .state('SocientyRules', {
            cache: false,
            url: "/UI/SocientyRules?Lang",
            title: 'SocientyRules',
            templateUrl: '/Csmi/SocientyRules'
        })
        /**************************** RegisterFeatures ****************************/
        .state('RegisterFeatures', {
            cache: false,
            url: "/UI/RegisterFeatures?Lang",
            title: 'RegisterFeatures',
            templateUrl: '/Csmi/RegisterFeatures'
        })
        /**************************** RegisterHelp ****************************/
        .state('RegisterHelp', {
            cache: false,
            url: "/UI/RegisterHelp?Lang",
            title: 'RegisterHelp',
            templateUrl: '/Csmi/RegisterHelp'
        })
        /**************************** Tarefe ****************************/
        .state('TarefeConference', {
            cache: false,
            url: "/UI/TarefeConference?Lang",
            title: 'TarefeConference',
            templateUrl: '/Csmi/TarefeConference'
        })
        .state('TarefeSociety', {
            cache: false,
            url: "/UI/TarefeSociety?Lang",
            title: 'TarefeSociety',
            templateUrl: '/Csmi/TarefeSociety'
        })
        /**************************** HamayeshActive ****************************/
        .state('HamayeshActive', {
            cache: false,
            url: "/UI/HamayeshActive?Lang",
            title: 'HamayeshActive',
            templateUrl: '/Csmi/HamayeshActive',
        })
        /**************************** Paper ****************************/
        .state('ConferenceList', {
            cache: false,
            url: "/UI/ConferenceList?Lang",
            title: 'ConferenceList',
            templateUrl: '/Csmi/ConferenceList'
        })
        .state('ArticleList', {
            cache: false,
            url: "/UI/ArticleList?Lang&ConferenceID&FieldID&PosterArticle",
            title: 'ArticleList',
            templateUrl: function (stateParams) {
                return "/Csmi/ArticleList?ConferenceID=" + stateParams.ConferenceID + "&FieldID=" + stateParams.FieldID + "&PosterArticle=" + stateParams.PosterArticle;
            }
        })
        .state('VirtualBoard', {
            cache: false,
            url: "/UI/VirtualBoard?Lang&ConferenceID&FieldID&PosterArticle",
            title: 'ArticleList',
            templateUrl: function (stateParams) {
                return "/Csmi/ArticleList?ConferenceID=" + stateParams.ConferenceID + "&FieldID=" + stateParams.FieldID + "&PosterArticle=" + stateParams.PosterArticle;
            }
        })
        .state('ArticleDetails', {
            cache: false,
            url: "/UI/ArticleDetails?Lang&ArticleID",
            title: 'ArticleDetails',
            templateUrl: function (stateParams) {
                return "/Csmi/ArticleDetails?ArticleID=" + stateParams.ArticleID;
            }
        })
        /**************************** PaperArbiterList ****************************/
        .state('PaperArbiterList', {
            cache: false,
            url: "/UI/PaperArbiterList?Lang",
            title: 'PaperArbiterList',
            templateUrl: '/Csmi/PaperArbiterList'
        })
        //**************************** NEWS ****************************//
        .state('NewsList', {
            cache: false,
            url: "/UI/NewsList?Lang&pageNumber&year",
            title: 'NewsList',
            templateUrl: function (stateParams) {
                return "/NewsModule/NewsList?pageNumber=" + stateParams.pageNumber + "&year=" + stateParams.year;
            }
        })
        .state('NewsDetails', {
            url: "/UI/NewsDetails?Lang&id",
            title: 'News',
            templateUrl: function (stateParams) {
                return "/NewsModule/NewsDetails/" + stateParams.id;
            }
        })
        //**************************** Poll ****************************//
        .state('Poll', {
            url: "/UI/Poll?Lang",
            title: 'Poll',
            templateUrl: "Csmi/Poll"
        })
        //**************************** Election ****************************//
        .state('ElectionResult', {
            url: "/UI/ElectionResult?id",
            title: 'ElectionResult',
            templateUrl: function (stateParams) {
                return "/Csmi/ElectionResult?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['/Areas/Admin/assets/js/plugins/highchart/highcharts.js']
                        }
                    ]);
                }
            }
        })
    $locationProvider.html5Mode(true);
    $locationProvider.hashPrefix('!');
}
angular
    .module('CSMI')
    .config(config)
    .run(function ($rootScope, $state, $filter, $templateCache, $location, $window) {
        $rootScope.$state = $state;
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, options) {
            toParams.Lang = getCookie('Lang');
            if ($state.current.name !== '') {
                if (jPM !== null) {
                    jPM.close();
                }
            }
        })
        $rootScope.$on('$stateChangeError', function (e, toState, toParams, fromState, fromParams, result) {

            if (result.data.error === "Forbidden") {
                $('#NoLoginHeaderPanel').show();
                $('#LoginHeaderPanel').hide();
                $state.go('Login');;
            }
        });
        $rootScope.$on('$stateChangeSuccess', function () {

            //if ($state.current.name === 'Home') {
            //    document.title = $filter('translate')('HeaderTitle') + '-' + $filter('translate')($state.current.title);
            //    $('meta[name=description]').attr('content', $filter('translate')('HeaderTitle') + '-' + $filter('translate')($state.current.title));
            //}
            //else {
            //    document.title = $filter('translate')($state.current.title);
            //    $('meta[name=description]').attr('content', $filter('translate')($state.current.title));
            //}
            //$('meta[name=keywords]').attr('content', $filter('translate')('HeaderTitle') + ',' + $filter('translate')($state.current.title));
            //if ($state.current.name !== 'SocietyChart') {
            //    $('link[href="/assets/css/SocietyChart.css"]').remove();
            //}
            if ($state.current.name !== 'UserProfile.PayConference') {
                $templateCache.removeAll();
            }
            $templateCache.put("select2/choices.tpl.html", "<ul tabindex=\"-1\" class=\"ui-select-choices ui-select-choices-content select2-results\"><li class=\"ui-select-choices-group\" ng-class=\"{\'select2-result-with-children\': $select.choiceGrouped($group) }\"><div ng-show=\"$select.choiceGrouped($group)\" class=\"ui-select-choices-group-label select2-result-label\" ng-bind=\"$group.name\"></div><ul id=\"ui-select-choices-{{ $select.generatedId }}\" ng-class=\"{\'select2-result-sub\': $select.choiceGrouped($group), \'select2-result-single\': !$select.choiceGrouped($group) }\"><li role=\"option\" ng-attr-id=\"ui-select-choices-row-{{ $select.generatedId }}-{{$index}}\" class=\"ui-select-choices-row\" ng-class=\"{\'select2-highlighted\': $select.isActive(this), \'select2-disabled\': $select.isDisabled(this)}\"><div class=\"select2-result-label ui-select-choices-row-inner\"></div></li></ul></li></ul>");
            $templateCache.put("select2/match-multiple.tpl.html", "<span class=\"ui-select-match\"><li class=\"ui-select-match-item select2-search-choice\" ng-repeat=\"$item in $select.selected track by $index\" ng-class=\"{\'select2-search-choice-focus\':$selectMultiple.activeMatchIndex === $index, \'select2-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span uis-transclude-append=\"\"></span> <a href=\"javascript:;\" class=\"ui-select-match-close select2-search-choice-close\" ng-click=\"$selectMultiple.removeChoice($index)\" tabindex=\"-1\"></a></li></span>");
            $templateCache.put("select2/match.tpl.html", "<a class=\"select2-choice ui-select-match\" ng-class=\"{\'select2-default\': $select.isEmpty()}\" ng-click=\"$select.toggle($event)\" aria-label=\"{{ $select.baseTitle }} select\"><span ng-show=\"$select.isEmpty()\" class=\"select2-chosen\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty()\" class=\"select2-chosen\" ng-transclude=\"\"></span> <abbr ng-if=\"$select.allowClear && !$select.isEmpty()\" class=\"select2-search-choice-close\" ng-click=\"$select.clear($event)\"></abbr> <span class=\"select2-arrow ui-select-toggle\"><b></b></span></a>");
            $templateCache.put("select2/no-choice.tpl.html", "<div class=\"ui-select-no-choice dropdown\" ng-show=\"$select.items.length == 0\"><div class=\"dropdown-content\"><div data-selectable=\"\" ng-transclude=\"\"></div></div></div>");
            $templateCache.put("select2/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple select2 select2-container select2-container-multi\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled}\"><ul class=\"select2-choices\"><span class=\"ui-select-match\"></span><li class=\"select2-search-field\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" aria-activedescendant=\"ui-select-choices-row-{{ $select.generatedId }}-{{ $select.activeIndex }}\" class=\"select2-input ui-select-search\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-hide=\"$select.disabled\" ng-model=\"$select.search\" ng-click=\"$select.activate()\" style=\"width: 34px;\" ondrop=\"return false;\"></li></ul><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open || $select.items.length === 0}\"><div class=\"ui-select-choices\"></div></div></div>");
            $templateCache.put("select2/select.tpl.html", "<div class=\"ui-select-container select2 select2-container\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled, \'select2-container-active\': $select.focus, \'select2-allowclear\': $select.allowClear && !$select.isEmpty()}\"><div class=\"ui-select-match\"></div><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open}\"><div class=\"search-container\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled, \'select2-search\':$select.searchEnabled}\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" ng-class=\"{\'select2-active\': $select.refreshing}\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" class=\"ui-select-search select2-input\" ng-model=\"$select.search\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div></div>");
        });
        $rootScope.$on('$viewContentLoaded', function (event) {
            if ($window.ga != undefined) {
                $window.ga('send', 'pageview', { page: $location.url() });
            }
            $("#Mytop").themeRefresh();
        });

    });
