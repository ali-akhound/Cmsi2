/** 
 * INSPINIA - Responsive Admin Theme
 *
 * Inspinia theme use AngularUI Router to manage routing and views
 * Each view are defined as state.
 * Initial there are written state for all view in theme.
 *
 */
function config($stateProvider, $urlRouterProvider, $locationProvider, $ocLazyLoadProvider, IdleProvider, KeepaliveProvider, blockUIConfig) {

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

    $urlRouterProvider.otherwise("HomeAdmin");

    $ocLazyLoadProvider.config({
        /* Set to true if you want to see what and when is dynamically loaded /*/
        debug: false
    });

    $stateProvider
        .state('index', {
            url: "/index",
            title: '',
            templateUrl: "/Admin/PublicAdmin/Index"
        })
        .state('HomeAdmin', {
            url: "/HomeAdmin?Mode",
            title: 'Home',
            templateUrl: function (stateParams) {
                if (stateParams.Mode == undefined)
                    return "/PublicAdmin/Home?Mode=Admin";
                else
                    return "/PublicAdmin/Home?Mode=" + stateParams.Mode;
            }
        })
        .state('ForgotPassword', {
            url: "/ForgotPassword",
            title: 'ForgotPassword',
            templateUrl: "/Admin/PublicAdmin/ForgotPassword",
        })
        .state('ResetPassword', {
            cache: false,
            url: "/ResetPassword?Lang&id&code",
            title: 'ResetPassword',
            templateUrl: function (stateParams) {
                return "/PublicAdmin/ResetPassword?id=" + stateParams.id + "&code=" + stateParams.code;
            }
        })
        .state('About', {
            abstract: true,
            url: "/About",
            templateUrl: "/Admin/PublicAdmin/about",
        })
        .state('ChangePass', {
            title: 'ChangePass',
            url: "/ChangePass",
            templateUrl: "/Admin/PublicAdmin/ChangePass",
        })
        /**************************** ROLE ****************************/
        .state('role', {
            url: "/role?id",
            title: 'ROLE',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/Role?id=" + stateParams.id;
            }
        })
        .state('rolemanagement', {
            cache: false,
            url: "/RoleManagement",
            title: 'ROlEMANAGEMENT',
            templateUrl: "/Admin/PublicAdmin/RoleManagement"
        })
        /**************************** ContactUs ****************************/
        .state('ContactUsMessages', {
            cache: false,
            url: "/ContactUsManagement",
            title: 'ContactUsManagement',
            templateUrl: "/Admin/PublicAdmin/ContactUsManagement"
        })
        /**************************** DynamicPage ****************************/
        .state('DynamicPageManagement', {
            cache: false,
            url: "/DynamicPageManagement",
            title: 'DynamicPageManagement',
            templateUrl: "/Admin/PublicAdmin/DynamicPageManagement"
        })
        .state('DynamicPage', {
            url: "/DynamicPage?id",
            title: 'DynamicPage',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/DynamicPage?id=" + stateParams.id;
            }
        })
        /**************************** User ****************************/
        .state('UserManagement', {
            cache: false,
            url: "/UserManagement",
            title: 'UserManagement',
            templateUrl: "/Admin/PublicAdmin/UserManagement"
        })
        .state('User', {
            url: "/User?id",
            title: 'User',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/UserView?id=" + stateParams.id;
            }
        })
        .state('UserRole', {
            url: "/UserRole?id",
            title: 'UserRole',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/UserRole?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }

        })
        .state('SysRoleModule', {
            url: "/SysRoleModule?roleID",
            title: 'SysRoleModule',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/SysRoleModule?roleID=" + stateParams.roleID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['/Areas/Admin/assets/css/plugins/iCheck/custom.css', '/Areas/Admin/assets/js/plugins/iCheck/icheck.min.js']
                        }
                    ]);
                }
            }

        })
        /**************************** NEWS ****************************/
        .state('NewsManagement', {
            cache: false,
            url: "/NewsManagement",
            title: 'NewsManagement',
            templateUrl: "/Admin/News/NewsManagement"
        })
        .state('News', {
            url: "/News?id",
            title: 'News',
            templateUrl: function (stateParams) {
                return "/Admin/News/News?id=" + stateParams.id;
            }
        })
        /**************************** MailTemplate *******************/
        .state('MailTemplate', {
            url: "/MailTemplate?id",
            title: 'MailTemplate',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/MailTemplate?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ngTagsInput',
                            files: ['/Areas/Admin/assets/js/plugins/ngTags/ng-tags-input.min.js', '/Areas/Admin/assets/css/plugins/ngTags/ng-tags-input-custom.min.css']
                        }
                    ]);
                }
            }
        })
        /**************************** FAQ ****************************/
        .state('FAQManagement', {
            cache: false,
            url: "/FAQManagement",
            title: 'FAQManagement',
            templateUrl: "/Admin/FAQ/FAQManagement"
        })
        .state('FAQ', {
            url: "/FAQ?id",
            title: 'FAQ',
            templateUrl: function (stateParams) {
                return "/Admin/FAQ/FAQ?id=" + stateParams.id;
            }
        })
        /**************************** Poll ****************************/
        .state('PollManagement', {
            cache: false,
            url: "/PollManagement",
            title: 'PollManagement',
            templateUrl: "/Admin/Poll/PollManagement"
        })
        /************************ Gallery ****************************/
        .state('GalleryAlbumManagement', {
            cache: false,
            url: "/GalleryAlbumManagement",
            title: 'GalleryAlbumManagement',
            templateUrl: "/Admin/GalleryAlbum/GalleryAlbumManagement"
        })
        .state('GalleryAlbum', {
            url: "/GalleryAlbum?id",
            title: 'GalleryAlbum',
            templateUrl: function (stateParams) {
                return "/Admin/GalleryAlbum/GalleryAlbum?id=" + stateParams.id;
            }
        })
        .state('GalleryImageManagement', {
            cache: false,
            url: "/GalleryImageManagement?AlbumID",
            title: 'GalleryImageManagement',
            templateUrl: function (stateParams) {
                return "/Admin/GalleryImage/GalleryImageManagement?AlbumID=" + stateParams.AlbumID;
            }
        })
        .state('GalleryImage', {
            url: "/GalleryImage?id&AlbumID",
            title: 'GalleryImage',
            templateUrl: function (stateParams) {
                return "/Admin/GalleryImage/GalleryImage?id=" + stateParams.id + "&AlbumID=" + stateParams.AlbumID;
            }
        })
        /**************************** Doctors ****************************/
        .state('CityZonesManagement', {
            cache: false,
            url: "/Companies/CityZonesManagement",
            title: 'CityZonesManagement',
            templateUrl: "/Admin/Companies/CityZonesManagement"
        })
        .state('CityZones', {
            url: "/Companies/CityZones?id",
            title: 'CityZones',
            templateUrl: function (stateParams) {
                return "/Admin/Companies/CityZones?id=" + stateParams.id;
            }
        })
        .state('FeaturesManagement', {
            cache: false,
            url: "/Companies/FeaturesManagement?CatID",
            title: 'FeaturesManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Companies/FeaturesManagement?CatID=" + stateParams.CatID;
            }
        })
        .state('Companies', {
            url: "/Companies/Companies?id",
            title: 'Companies',
            templateUrl: function (stateParams) {
                return "/Admin/Companies/Companies?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ngMap',
                            files: [
                                'http://maps.google.com/maps/api/js?key=AIzaSyDaaqYvaPxgEalEkg16DzndV0gEDRJst0w',
                                '/assets/js/Pezeshkan/plugins/ngmap/ng-map.min.js',
                            ]

                        },
                        {
                            files: ['/Areas/Admin/assets/css/plugins/dropzone/basic.css', '/Areas/Admin/assets/css/plugins/dropzone/dropzone.css', '/Areas/Admin/assets/js/plugins/dropzone/dropzone.js']
                        },
                        {
                            files: ['/Areas/Admin/assets/js/plugins/jasny/jasny-bootstrap.min.js', '/Areas/Admin/assets/css/plugins/jasny/jasny-bootstrap.min.css', '/Areas/Admin/assets/js/object-traverse.min.js', '/Areas/Admin/assets/js/object-to-formdata.min.js']
                        },
                        {
                            name: 'ui.switchery',
                            files: ['/Areas/Admin/assets/css/plugins/switchery/switchery.css', '/Areas/Admin/assets/js/plugins/switchery/switchery.js', '/Areas/Admin/assets/js/plugins/switchery/ng-switchery.js']
                        }
                    ]);
                }
            }
        })
        .state('CompaniesManagement', {
            cache: false,
            url: "/Companies/CompaniesManagement",
            title: 'FeaturesManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Companies/CompaniesManagement";
            }
        })
        .state('Features', {
            cache: false,
            url: "/Companies/Features?id",
            title: 'Features',
            templateUrl: function (stateParams) {
                return "/Admin/Companies/Features?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }
        })
        /**************************** Csmi ****************************/
        /*Referee */
        .state('RefereeManagement', {
            cache: false,
            url: "/Conference/RefereeManagement",
            title: 'RefereeManagement',
            templateUrl: "/Admin/Referee/RefereeManagement"
        })
        .state('Referee', {
            url: "/Conference/Referee?id",
            title: 'Referee',
            templateUrl: function (stateParams) {
                return "/Admin/Referee/Referee?id=" + stateParams.id;
            }
        })
        /* Article */
        .state('ArticleManagement', {
            cache: false,
            url: "/Conference/ArticleManagement?ConferenceID",
            title: 'ArticleManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Article/ArticleManagement?ConferenceID=" + stateParams.ConferenceID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }
        })
        .state('Article', {
            url: "/Conference/Article?id",
            title: 'Article',
            templateUrl: function (stateParams) {
                return "/Admin/Article/Article?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['/Areas/Admin/assets/css/plugins/iCheck/custom.css', '/Areas/Admin/assets/js/plugins/iCheck/icheck.min.js']
                        }
                    ]);
                }
            }
        })
        /* Conference */
        .state('ConferenceReferee', {
            url: "/Conference/ConferenceReferee?ConferenceID",
            title: 'ConferenceReferee',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceReferee/ConferenceReferee?ConferenceID=" + stateParams.ConferenceID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }

        })
        .state('ConferenceRefereeManagement', {
            cache: false,
            url: "/Conference/ConferenceRefereeManagement?ConferenceID",
            title: 'ConferenceRefereeManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceReferee/ConferenceRefereeManagement?ConferenceID=" + stateParams.ConferenceID;
            }
        })
        .state('ConferenceArticleManagement', {
            cache: false,
            url: "/Conference/ConferenceArticleManagement?ConferenceID",
            title: 'ConferenceArticleManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceReferee/ConferenceArticleManagement?ConferenceID=" + stateParams.ConferenceID;
            }
        })
        .state('ConferenceExecutor', {
            url: "/Conference/ConferenceExecutor?ConferenceID",
            title: 'ConferenceExecutor',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceExecutor/ConferenceExecutor?ConferenceID=" + stateParams.ConferenceID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }

        })
        .state('ConferenceExecutorManagement', {
            cache: false,
            url: "/Conference/ConferenceExecutorManagement?ConferenceID",
            title: 'ConferenceExecutorManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceExecutor/ConferenceExecutorManagement?ConferenceID=" + stateParams.ConferenceID;
            }
        })
        .state('ConferenceScientificSecretary', {
            url: "/Conference/ConferenceScientificSecretary?ConferenceID",
            title: 'ConferenceScientificSecretary',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceScientificSecretary/ConferenceScientificSecretary?ConferenceID=" + stateParams.ConferenceID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }

        })
        .state('ConferenceScientificSecretaryManagement', {
            cache: false,
            url: "/Conference/ConferenceScientificSecretaryManagement?ConferenceID",
            title: 'ConferenceScientificSecretaryManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceScientificSecretary/ConferenceScientificSecretaryManagement?ConferenceID=" + stateParams.ConferenceID;
            }
        })
        .state('ConferenceCategory', {
            url: "/Conference/Category?id&TableName&ParentID",
            title: 'ConferenceCategory',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/ConferenceCategory?id=" + stateParams.id + "&TableName=" + stateParams.TableName + "&ParentID=" + stateParams.ParentID;
            }
        })
        .state('ConferenceCategoryManagement', {
            cache: false,
            url: "/Conference/ConferenceCategoryManagement?TableName&ParentID",
            title: 'ConferenceCategoryManagement',
            templateUrl: function (stateParams) {
                return "/Admin/PublicAdmin/ConferenceCategoryManagement?TableName=" + stateParams.TableName + "&ParentID=" + stateParams.ParentID;
            }
        })
        .state('ConferenceDefinedCategory', {
            url: "/Conference/ConferenceDefinedCategory?ConferenceID",
            title: 'ConferenceDefinedCategory',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceDefinedCategory/ConferenceDefinedCategory?ConferenceID=" + stateParams.ConferenceID;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'ui.select',
                            files: ['/Areas/Admin/assets/js/plugins/ui-select/select.min.js'
                                , '/Areas/Admin/assets/css/plugins/ui-select/select2.css'
                                , '/Areas/Admin/assets/css/plugins/ui-select/MySelect2Custom.css'
                            ]
                        }
                    ]);
                }
            }

        })
        .state('ConferenceDefinedCategoryManagement', {
            cache: false,
            url: "/Conference/ConferenceDefinedCategoryManagement?ConferenceID",
            title: 'ConferenceDefinedCategoryManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceDefinedCategory/ConferenceDefinedCategoryManagement?ConferenceID=" + stateParams.ConferenceID;
            }
        })
        .state('ConferenceManagement', {
            cache: false,
            url: "/Conference/ConferenceManagement",
            title: 'ConferenceManagement',
            templateUrl: "/Admin/Conference/ConferenceManagement"
        })
        .state('Conference', {
            url: "/Conference/Conference?id",
            title: 'Conference',
            templateUrl: function (stateParams) {
                return "/Admin/Conference/Conference?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            name: 'dynamicNumber',
                            files: [
                                '/Areas/Admin/assets/js/plugins/angular-dynamic-number/dynamic-number.min.js',
                            ]

                        }
                    ]);
                }
            }
        })
        /* Executor */
        .state('ExecutorManagement', {
            cache: false,
            url: "/Conference/ExecutorManagement",
            title: 'ExecutorManagement',
            templateUrl: "/Admin/Executor/ExecutorManagement"
        })
        .state('Executor', {
            url: "/Conference/Executor?id",
            title: 'Executor',
            templateUrl: function (stateParams) {
                return "/Admin/Executor/Executor?id=" + stateParams.id;
            }
        })
        /* ScientificSecretary */
        .state('ScientificSecretaryManagement', {
            cache: false,
            url: "/Conference/ScientificSecretaryManagement",
            title: 'ScientificSecretaryManagement',
            templateUrl: "/Admin/ScientificSecretary/ScientificSecretaryManagement"
        })
        .state('ScientificSecretary', {
            url: "/Conference/ScientificSecretary?id",
            title: 'ScientificSecretary',
            templateUrl: function (stateParams) {
                return "/Admin/ScientificSecretary/ScientificSecretary?id=" + stateParams.id;
            }
        })
        /* ArticleStatus */
        .state('ArticleStatusManagement', {
            cache: false,
            url: "/Conference/ArticleStatusManagement?id",
            title: 'ArticleStatusManagement',
            templateUrl: "/Admin/ArticleStatus/ArticleStatusManagement"
        })
        .state('ArticleStatus', {
            url: "/Conference/ArticleStatus?id",
            title: 'ArticleStatus',
            templateUrl: function (stateParams) {
                return "/Admin/ArticleStatus/ArticleStatus?id=" + stateParams.id;
            }
        })
        /* RefereeStatus */
        .state('RefereeStatusManagement', {
            cache: false,
            url: "/Conference/RefereeStatusManagement?id",
            title: 'RefereeStatusManagement',
            templateUrl: "/Admin/RefereeStatus/RefereeStatusManagement"
        })
        .state('RefereeStatus', {
            url: "/Conference/RefereeStatus?id",
            title: 'RefereeStatus',
            templateUrl: function (stateParams) {
                return "/Admin/RefereeStatus/RefereeStatus?id=" + stateParams.id;
            }
        })
        /* ArticlePresentType */
        .state('ArticlePresentTypeManagement', {
            cache: false,
            url: "/Conference/ArticlePresentTypeManagement?id",
            title: 'ArticlePresentTypeManagement',
            templateUrl: "/Admin/ArticlePresentType/ArticlePresentTypeManagement"
        })
        .state('ArticlePresentType', {
            url: "/Conference/ArticlePresentType?id",
            title: 'ArticlePresentType',
            templateUrl: function (stateParams) {
                return "/Admin/ArticlePresentType/ArticlePresentType?id=" + stateParams.id;
            }
        })
        /* RefereeQuestion */
        .state('RefereeQuestionManagement', {
            cache: false,
            url: "/Conference/RefereeQuestionManagement?id",
            title: 'RefereeQuestionManagement',
            templateUrl: "/Admin/RefereeQuestion/RefereeQuestionManagement"
        })
        .state('RefereeQuestion', {
            url: "/Conference/RefereeQuestion?id",
            title: 'RefereeQuestion',
            templateUrl: function (stateParams) {
                return "/Admin/RefereeQuestion/RefereeQuestion?id=" + stateParams.id;
            },
            resolve: {
                loadPlugin: function ($ocLazyLoad) {
                    return $ocLazyLoad.load([
                        {
                            files: ['/Areas/Admin/assets/css/plugins/iCheck/custom.css', '/Areas/Admin/assets/js/plugins/iCheck/icheck.min.js']
                        }
                    ]);
                }
            }
        })
        /* RefereeArticle */
        .state('RefereeArticle', {
            url: "/Conference/RefereeArticle?id",
            title: 'RefereeArticle',
            templateUrl: function (stateParams) {
                return "/Admin/RefereeArticle/RefereeArticle?id=" + stateParams.id;
            }
        })
        .state('RefereeArticleManagement', {
            cache: false,
            url: "/Conference/RefereeArticleManagement?Receive",
            title: 'RefereeArticleManagement',
            templateUrl: function (stateParams) {
                return "/Admin/RefereeArticle/RefereeArticleManagement?Receive=" + stateParams.Receive;
            }
        })
        /* SocietyMember */
        .state('SocietyMemberManagement', {
            cache: false,
            url: "/SocietyMemberManagement?PeriodID",
            title: 'SocietyMemberManagement',
            templateUrl: function (stateParams) {
                return "/Admin/SocietyMember/SocietyMemberManagement?PeriodID=" + stateParams.PeriodID;
            }
        })
        .state('SocietyMember', {
            url: "/SocietyMember?id&PeriodID",
            title: 'SocietyMember',
            templateUrl: function (stateParams) {
                return "/Admin/SocietyMember/SocietyMember?id=" + stateParams.id + '' + "&PeriodID=" + stateParams.PeriodID;
            }
        })
        .state('SocietyMemberPeriodManagement', {
            cache: false,
            url: "/SocietyMemberPeriodManagement",
            title: 'SocietyMemberPeriodManagement',
            templateUrl: "/Admin/SocietyMemberPeriod/SocietyMemberPeriodManagement"
        })
        .state('SocietyMemberPeriod', {
            url: "/SocietyMemberPeriod?id",
            title: 'SocietyMemberPeriod',
            templateUrl: function (stateParams) {
                return "/Admin/SocietyMemberPeriod/SocietyMemberPeriod?id=" + stateParams.id;
            }
        })
        .state('SocietyExecutorManagement', {
            cache: false,
            url: "/SocietyExecutorManagement",
            title: 'SocietyExecutorManagement',
            templateUrl: "/Admin/SocietyExecutor/SocietyExecutorManagement"
        })
        .state('SocietyExecutor', {
            url: "/SocietyExecutor?id",
            title: 'SocietyExecutor',
            templateUrl: function (stateParams) {
                return "/Admin/SocietyExecutor/SocietyExecutor?id=" + stateParams.id;
            }
        })
        .state('BookManagement', {
            cache: false,
            url: "/BookManagement",
            title: 'BookManagement',
            templateUrl: "/Admin/Book/BookManagement"
        })
        .state('Book', {
            url: "/Book?id",
            title: 'Book',
            templateUrl: function (stateParams) {
                return "/Admin/Book/Book?id=" + stateParams.id;
            }
        })
        /* PackageName */
        .state('PackageName', {
            url: "/Conference/PackageName?id",
            title: 'PackageName',
            templateUrl: function (stateParams) {
                return "/Admin/PackageName/PackageName?id=" + stateParams.id;
            }
        })
        .state('PackageNameManagement', {
            cache: false,
            url: "/Conference/PackageNameManagement",
            title: 'PackageNameManagement',
            templateUrl: function (stateParams) {
                return "/Admin/PackageName/PackageNameManagement";
            }
        })
        /* ConferenceCompanion */
        .state('ConferenceCompanion', {
            url: "/Conference/ConferenceCompanion?id",
            title: 'ConferenceCompanion',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceCompanion/ConferenceCompanion?id=" + stateParams.id;
            }
        })
        .state('ConferenceCompanionManagement', {
            cache: false,
            url: "/Conference/ConferenceCompanionManagement?ConferenceID&UserID",
            title: 'ConferenceCompanionManagement',
            templateUrl: function (stateParams) {
                return "/Admin/ConferenceCompanion/ConferenceCompanionManagement?ConferenceID=" + stateParams.ConferenceID + '&UserID=' + stateParams.UserID;
            }
        })
        /* CompanyPackageName */
        .state('CompanyPackageName', {
            url: "/Conference/CompanyPackageName?id",
            title: 'CompanyPackageName',
            templateUrl: function (stateParams) {
                return "/Admin/CompanyPackageName/CompanyPackageName?id=" + stateParams.id;
            }
        })
        .state('CompanyPackageNameManagement', {
            cache: false,
            url: "/Conference/CompanyPackageNameManagement",
            title: 'CompanyPackageNameManagement',
            templateUrl: function (stateParams) {
                return "/Admin/CompanyPackageName/CompanyPackageNameManagement";
            }
        })
        /* CompanyPackageName */
        .state('PaymentManagement', {
            cache: false,
            url: "/Conference/PaymentManagement",
            title: 'PaymentManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Payment/PaymentManagement";
            }
        })
        .state('SocietyPayment', {
            cache: false,
            url: "/SocietyPayment",
            title: 'SocietyPayment',
            templateUrl: function (stateParams) {
                return "/Admin/SocietyPayment/SocietyPaymentManagement";
            }
        })
        /*Election */
        .state('ElectionManagement', {
            cache: false,
            url: "/ElectionManagement",
            title: 'ElectionManagement',
            templateUrl: "/Admin/Election/ElectionManagement"
        })
        .state('ElectionVoterManagement', {
            cache: false,
            url: "/ElectionVoterManagement?id",
            title: 'ElectionVoterManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Election/ElectionVoterManagement?id=" + stateParams.id;
            }
        })
        .state('Election', {
            url: "/Election?id",
            title: 'Election',
            templateUrl: function (stateParams) {
                return "/Admin/Election/Election?id=" + stateParams.id;
            }
        })
        .state('ElectionResult', {
            url: "/ElectionResult?id",
            title: 'ElectionResult',
            templateUrl: function (stateParams) {
                return "/Admin/Election/ElectionResult?id=" + stateParams.id;
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
        .state('ElectionVoterDiagram', {
            url: "/ElectionVoterDiagram?id",
            title: 'ElectionVoterDiagram',
            templateUrl: function (stateParams) {
                return "/Admin/Election/ElectionVoterDiagram?id=" + stateParams.id;
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
        /*Candidate */
        .state('CandidateManagement', {
            cache: false,
            url: "/CandidateManagement?ElectionID",
            title: 'CandidateManagement',
            templateUrl: function (stateParams) {
                return "/Admin/Candidate/CandidateManagement?ElectionID=" + stateParams.ElectionID;
            }
        })
        .state('Candidate', {
            url: "/Candidate?id&ElectionID",
            title: 'Candidate',
            templateUrl: function (stateParams) {
                if (stateParams.ElectionID == undefined)
                    return "/Admin/Candidate/Candidate?id=" + stateParams.id;
                else
                    return "/Admin/Candidate/Candidate?id=" + stateParams.id + '&ElectionID=' + stateParams.ElectionID;
            }
        })
        /*Vote */
        .state('Vote', {
            cache: false,
            url: "/Vote",
            title: 'Vote',
            templateUrl: function (stateParams) {
                return "/Admin/Vote/Vote";
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
    /*$locationProvider.html5Mode(true); */
}
angular
    .module('inspinia')
    .config(['$stateProvider', '$urlRouterProvider', '$locationProvider', '$ocLazyLoadProvider', 'IdleProvider', 'KeepaliveProvider', 'blockUIConfig', config])
    .run(['$rootScope', '$state', '$filter', '$templateCache',
        function ($rootScope, $state, $filter, $templateCache) {
            $rootScope.$state = $state;
            $rootScope.$on('$stateChangeSuccess', function () {
                document.title = $filter('translate')($state.current.title);
                $templateCache.put("select2/choices.tpl.html", "<ul tabindex=\"-1\" class=\"ui-select-choices ui-select-choices-content select2-results\"><li class=\"ui-select-choices-group\" ng-class=\"{\'select2-result-with-children\': $select.choiceGrouped($group) }\"><div ng-show=\"$select.choiceGrouped($group)\" class=\"ui-select-choices-group-label select2-result-label\" ng-bind=\"$group.name\"></div><ul id=\"ui-select-choices-{{ $select.generatedId }}\" ng-class=\"{\'select2-result-sub\': $select.choiceGrouped($group), \'select2-result-single\': !$select.choiceGrouped($group) }\"><li role=\"option\" ng-attr-id=\"ui-select-choices-row-{{ $select.generatedId }}-{{$index}}\" class=\"ui-select-choices-row\" ng-class=\"{\'select2-highlighted\': $select.isActive(this), \'select2-disabled\': $select.isDisabled(this)}\"><div class=\"select2-result-label ui-select-choices-row-inner\"></div></li></ul></li></ul>");
                $templateCache.put("select2/match-multiple.tpl.html", "<span class=\"ui-select-match\"><li class=\"ui-select-match-item select2-search-choice\" ng-repeat=\"$item in $select.selected track by $index\" ng-class=\"{\'select2-search-choice-focus\':$selectMultiple.activeMatchIndex === $index, \'select2-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span uis-transclude-append=\"\"></span> <a href=\"javascript:;\" class=\"ui-select-match-close select2-search-choice-close\" ng-click=\"$selectMultiple.removeChoice($index)\" tabindex=\"-1\"></a></li></span>");
                $templateCache.put("select2/match.tpl.html", "<a class=\"select2-choice ui-select-match\" ng-class=\"{\'select2-default\': $select.isEmpty()}\" ng-click=\"$select.toggle($event)\" aria-label=\"{{ $select.baseTitle }} select\"><span ng-show=\"$select.isEmpty()\" class=\"select2-chosen\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty()\" class=\"select2-chosen\" ng-transclude=\"\"></span> <abbr ng-if=\"$select.allowClear && !$select.isEmpty()\" class=\"select2-search-choice-close\" ng-click=\"$select.clear($event)\"></abbr> <span class=\"select2-arrow ui-select-toggle\"><b></b></span></a>");
                $templateCache.put("select2/no-choice.tpl.html", "<div class=\"ui-select-no-choice dropdown\" ng-show=\"$select.items.length == 0\"><div class=\"dropdown-content\"><div data-selectable=\"\" ng-transclude=\"\"></div></div></div>");
                $templateCache.put("select2/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple select2 select2-container select2-container-multi\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled}\"><ul class=\"select2-choices\"><span class=\"ui-select-match\"></span><li class=\"select2-search-field\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" aria-activedescendant=\"ui-select-choices-row-{{ $select.generatedId }}-{{ $select.activeIndex }}\" class=\"select2-input ui-select-search\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-hide=\"$select.disabled\" ng-model=\"$select.search\" ng-click=\"$select.activate()\" style=\"width: 34px;\" ondrop=\"return false;\"></li></ul><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open || $select.items.length === 0}\"><div class=\"ui-select-choices\"></div></div></div>");
                $templateCache.put("select2/select.tpl.html", "<div class=\"ui-select-container select2 select2-container\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled, \'select2-container-active\': $select.focus, \'select2-allowclear\': $select.allowClear && !$select.isEmpty()}\"><div class=\"ui-select-match\"></div><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open}\"><div class=\"search-container\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled, \'select2-search\':$select.searchEnabled}\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" ng-class=\"{\'select2-active\': $select.refreshing}\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" class=\"ui-select-search select2-input\" ng-model=\"$select.search\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div></div>");
            });

        }
    ]);
