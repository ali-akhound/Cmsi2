/**
 * MainCtrl - controller
 * Contains several global data used in different view
 *
 */
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else {
        var expires = "";
    }
    document.cookie = name + "=" + value + expires + "; path=/";
}
function getCookie(name) {
    if (document.cookie.length > 0) {
        c_start = document.cookie.indexOf(name + "=");
        if (c_start != -1) {
            c_start = c_start + name.length + 1;
            c_end = document.cookie.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = document.cookie.length;
            }
            return unescape(document.cookie.substring(c_start, c_end));
        }
    }
    return "";
}
function MainCtrl($http, blockUIService) {

};

/**
 * GoogleMaps - Controller for data google maps
 */
function GoogleMaps($scope) {
    $scope.mapOptions = {
        zoom: 11,
        center: new google.maps.LatLng(40.6700, -73.9400),
        /* Style for Google Maps /*/
        styles: [{ "featureType": "water", "stylers": [{ "saturation": 43 }, { "lightness": -11 }, { "hue": "#0088ff" }] }, { "featureType": "road", "elementType": "geometry.fill", "stylers": [{ "hue": "#ff0000" }, { "saturation": -100 }, { "lightness": 99 }] }, { "featureType": "road", "elementType": "geometry.stroke", "stylers": [{ "color": "#808080" }, { "lightness": 54 }] }, { "featureType": "landscape.man_made", "elementType": "geometry.fill", "stylers": [{ "color": "#ece2d9" }] }, { "featureType": "poi.park", "elementType": "geometry.fill", "stylers": [{ "color": "#ccdca1" }] }, { "featureType": "road", "elementType": "labels.text.fill", "stylers": [{ "color": "#767676" }] }, { "featureType": "road", "elementType": "labels.text.stroke", "stylers": [{ "color": "#ffffff" }] }, { "featureType": "poi", "stylers": [{ "visibility": "off" }] }, { "featureType": "landscape.natural", "elementType": "geometry.fill", "stylers": [{ "visibility": "on" }, { "color": "#b8cb93" }] }, { "featureType": "poi.park", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.sports_complex", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.medical", "stylers": [{ "visibility": "on" }] }, { "featureType": "poi.business", "stylers": [{ "visibility": "simplified" }] }],
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    $scope.mapOptions2 = {
        zoom: 11,
        center: new google.maps.LatLng(40.6700, -73.9400),
        /* Style for Google Maps /*/
        styles: [{ "featureType": "all", "elementType": "all", "stylers": [{ "invert_lightness": true }, { "saturation": 10 }, { "lightness": 30 }, { "gamma": 0.5 }, { "hue": "#435158" }] }],
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    $scope.mapOptions3 = {
        center: new google.maps.LatLng(36.964645, -122.01523),
        zoom: 18,
        /* Style for Google Maps /*/
        MapTypeId: google.maps.MapTypeId.SATELLITE,
        styles: [{ "featureType": "road", "elementType": "geometry", "stylers": [{ "visibility": "off" }] }, { "featureType": "poi", "elementType": "geometry", "stylers": [{ "visibility": "off" }] }, { "featureType": "landscape", "elementType": "geometry", "stylers": [{ "color": "#fffffa" }] }, { "featureType": "water", "stylers": [{ "lightness": 50 }] }, { "featureType": "road", "elementType": "labels", "stylers": [{ "visibility": "off" }] }, { "featureType": "transit", "stylers": [{ "visibility": "off" }] }, { "featureType": "administrative", "elementType": "geometry", "stylers": [{ "lightness": 40 }] }],
        mapTypeId: google.maps.MapTypeId.SATELLITE
    };
    $scope.mapOptions4 = {
        zoom: 11,
        center: new google.maps.LatLng(40.6700, -73.9400),
        /* Style for Google Maps /*/
        styles: [{ "stylers": [{ "hue": "#18a689" }, { "visibility": "on" }, { "invert_lightness": true }, { "saturation": 40 }, { "lightness": 10 }] }],
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
}
/**
 * translateCtrl - Controller for translate
 */
function translateCtrl($translate, $scope, $state, $http, blockUI) {
    $scope.selected = getCookie('Lang');
    $translate.use(getCookie('Lang'));
    $scope.language = getCookie('Lang');
    $scope.isActive = function (langKey) {
        return $scope.selected === langKey;
    };
    $scope.changeLanguage = function (langKey) {
        $scope.selected = langKey;
        $translate.use(langKey);
        $scope.language = langKey;
        if (langKey != 'fa') {
            $('#customFa').attr('href', $('#customFa').attr('href').replace('customFa.css', 'customEn.css'));
        }
        else {
            $('#customFa').attr('href', $('#customFa').attr('href').replace('customEn.css', 'customFa.css'));
        }
        createCookie('Lang', langKey, 10);
        $state.params.Language = $scope.selected;
        $state.params.Lang = langKey;
        $state.go($state.current.name, $state.params, { reload: true, inherit: false });
        ajaxGet($http, footerDynamicUrl, null, blockUI, function (response) {
            $('#footerDynamic').html(response.data);
        });
        var isMobile = false; //initiate as false
        // device detection
        if (/(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|ipad|iris|kindle|Android|Silk|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino/i.test(navigator.userAgent)
            || /1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-/i.test(navigator.userAgent.substr(0, 4)))
            isMobile = true;
        if (isMobile) {
            blockUI.start();
            document.location.reload();
        }
    };
}
function passwordMeterCtrl($scope) {

    var options1 = {};
    options1.ui = {
        container: "#pwd-container1",
        showVerdictsInsideProgressBar: true,
        viewports: {
            progress: ".pwstrength_viewport_progress"
        }
    };
    options1.common = {
        debug: false,
    };
    $scope.option1 = options1;

    var options2 = {};
    options2.ui = {
        container: "#pwd-container2",
        showStatus: true,
        showProgressBar: false,
        viewports: {
            verdict: ".pwstrength_viewport_verdict"
        }
    };
    $scope.option2 = options2;

    var options3 = {};
    options3.ui = {
        container: "#pwd-container3",
        showVerdictsInsideProgressBar: true,
        viewports: {
            progress: ".pwstrength_viewport_progress2"
        }
    };
    options3.common = {
        debug: true,
        usernameField: "#username"
    };
    $scope.option3 = options3;

    var options4 = {};
    options4.ui = {
        container: "#pwd-container4",
        viewports: {
            progress: ".pwstrength_viewport_progress4",
            verdict: ".pwstrength_viewport_verdict4"
        }
    };
    options4.common = {
        zxcvbn: true,
        zxcvbnTerms: ['samurai', 'shogun', 'bushido', 'daisho', 'seppuku'],
        userInputs: ['#year', '#familyname']
    };
    $scope.option4 = options4;



}
function validateForm(selector) {
    /*$.validator.setDefaults({ ignore: [] }); /*/
    var currentForm = $(selector);
    currentForm.removeData('validator');
    currentForm.removeData('unobtrusiveValidation');
    $.validator.unobtrusive.parse(currentForm);
    return currentForm.valid();
}
function ShowToast(toaster, text, type) {
    /*
    success
    error
    warning
    */
    toaster.pop({
        type: type,
        body: text,
        showCloseButton: true,
        timeout: 5000
    });

}
function getModelAsFormData(data) {
    var dataAsFormData = new FormData();
    angular.forEach(data, function (value, key) {
        dataAsFormData.append(key, value);
    });
    return dataAsFormData;
};
function SubmitForm(url, form, data, $http, blockUIService, enableFormValidation, successCallback) {
    if (validateForm(form) || (!enableFormValidation && enableFormValidation != undefined)) {
        blockUIService.start();
        if ($(form).find('input[type="file"]').length > 0) {
            ajaxFileFormPost($http, url, data, blockUIService, successCallback)
        }
        else {
            ajaxPost($http, url, data, blockUIService, successCallback)
        }
    }
    else {
        ShowSweetAlert('لطفا مقادیر ورودی را بررسی نمایید', 'error');
    }
    /*debugger;
    blockUiTarget = blockUiTarget || $pageContainer;
    var thisForm = $(form);
    url = url || thisForm.attr('action');
    handleUnmask(thisForm);
    if (validateForm(thisForm)) {
        handleBlockUI(blockUiTarget);
        if (thisForm.find('input[type="file"]').length > 0) {
            $.ajax({
                type: "POST",
                url: url,
                data: new FormData(thisForm[0]),
                async: false,
                success: function (data) {
                    handleUnblockUI(blockUiTarget);
                    if (typeof (afterSuccess) == 'function')
                        afterSuccess(data);
                },
                cache: false,
                contentType: false,
                processData: false
            }).fail(function (xhr, err) {
                handleUnblockUI(blockUiTarget);
                if (xhr.statusCode == 403) {
                    window.location.replace("/authority/login");
                } else if (xhr.status == 404) {
                    alert('Your requested item was not found.');
                } else {
                    alert(xhr.statusText);
                }
            });
        } else {
            $.post(url, thisForm.serialize(), function (data) {
                handleUnblockUI(blockUiTarget);
                if (typeof (afterSuccess) == 'function') {
                    afterSuccess(data);
                }
            }).fail(function (xhr, err) {
                handleUnblockUI(blockUiTarget);
                if (xhr.status == 403) {
                    window.location.replace("/authority/login");
                } else if (xhr.status == 401) {
                    alert('Unauthorized! You have not enough permission to fulfill this action.');
                } else if (xhr.status == 404) {
                    alert('Your requested item was not found.');
                } else {
                    alert(xhr.statusText);
                }
            });
        }
    } /*/
};
function ajaxPost($http, url, data, blockUIService, successCallback, errorCallback, forgeryTokenID) {
    if (forgeryTokenID == null || forgeryTokenID == undefined) {
        forgeryTokenID = 'forgeryToken';
    }
    if (errorCallback == undefined || errorCallback == null) {
        errorCallback = function errorCallback(xhr, err) {
            blockUIService.stop();
            if (xhr.status == 403) {
                window.location.replace("/Admin/membership/login");
            } else if (xhr.status == 401) {
                ShowSweetAlert('Unauthorized! You have not enough permission to fulfill this action.', 'error');
            } else if (xhr.status == 404) {
                ShowSweetAlert('Your requested item was not found.', 'error');
            } else {
                if (xhr.data.error != undefined)
                    ShowSweetAlert(xhr.data.error, 'error');
                else
                    ShowSweetAlert(xhr.statusText, 'error');
            }
        }
    }
    $http({
        method: 'POST',
        url: url,
        data: data,
        dataType: "json",
        headers: { 'X-Requested-With': 'XMLHttpRequest', 'VerificationToken': $('#' + forgeryTokenID).val() }
    })
        .then(successCallback, errorCallback);
}
function ajaxFileFormPost($http, url, data, blockUIService, successCallback, errorCallback, forgeryTokenID) {
    if (forgeryTokenID == null || forgeryTokenID == undefined) {
        forgeryTokenID = 'forgeryToken';
    }
    if (errorCallback == undefined || errorCallback == null) {
        errorCallback = function errorCallback(xhr, err) {
            blockUIService.stop();
            if (xhr.status == 403) {
                window.location.replace("/Admin/membership/login");
            } else if (xhr.status == 401) {
                ShowSweetAlert('Unauthorized! You have not enough permission to fulfill this action.', 'error');
            } else if (xhr.status == 404) {
                ShowSweetAlert('Your requested item was not found.', 'error');
            } else {
                if (xhr.data.error != undefined)
                    ShowSweetAlert(xhr.data.error, 'error');
                else
                    ShowSweetAlert(xhr.statusText, 'error');
            }
        }
    }
    var fdata = new FormData();
    $('input[type=file]').each(function (a, b) {
        var fileInput = $('input[type=file]')[a];
        if (fileInput.files.length > 0) {
            var file = fileInput.files[0];
            fdata.append($(fileInput).attr('name'), file);
        }
    });
    fdata.append("viewModel", JSON.stringify(data));
    $http({
        method: 'POST',
        url: url,
        data: fdata,
        headers: { 'Content-Type': undefined, 'X-Requested-With': 'XMLHttpRequest', 'VerificationToken': $('#' + forgeryTokenID).val() }
    })
        .then(successCallback, errorCallback);
}
function ajaxGet($http, url, data, blockUIService, successCallback, errorCallback, forgeryTokenID) {
    if (forgeryTokenID == null || forgeryTokenID == undefined) {
        forgeryTokenID = 'forgeryToken';
    }
    if (errorCallback == undefined || errorCallback == null) {
        errorCallback = function errorCallback(xhr, err) {
            blockUIService.stop();
            if (xhr.status == 403) {
                window.location.replace("/Admin/membership/login");
            } else if (xhr.status == 401) {
                ShowSweetAlert('Unauthorized! You have not enough permission to fulfill this action.', 'error');
            } else if (xhr.status == 404) {
                ShowSweetAlert('Your requested item was not found.', 'error');
            } else {
                if (xhr.data.error != undefined)
                    ShowSweetAlert(xhr.data.error, 'error');
                else
                    ShowSweetAlert(xhr.statusText, 'error');
            }
        }
    }
    $http({
        method: 'GET',
        url: url,
        headers: { 'X-Requested-With': 'XMLHttpRequest', 'VerificationToken': $('#' + forgeryTokenID).val() }
    })
        .then(successCallback, errorCallback);
}
function handleDeleteAction($http, deleteUrl, ids, $state, $templateCache) {
    swal({
        title: "آیا از حذف مطمئن هستید؟",
        text: "بعد از حذف قادر به بازگردانی اطلاعات نخواهید بود",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "بلی",
        cancelButtonText: "انصراف",
        closeOnConfirm: false,
        showLoaderOnConfirm: true,
    }, function () {
        ajaxPost($http, deleteUrl, ids, null, function successCallback(response) {
            swal("Deleted!", "با موفقیت حذف شد", "success");
            $templateCache.removeAll();
            $state.reload();
        }, function errorCallback(xhr, err) {
            if (xhr.status == 403) {
                window.location.replace("/Admin/membership/login");
            } else if (xhr.status == 401) {
                swal("Cancelled", 'Unauthorized! You have not enough permission to fulfill this action.', "error");
            } else if (xhr.status == 404) {
                swal("Cancelled", 'Your requested item was not found.', "error");
            } else {
                if (xhr.data.error != undefined)
                    swal("Cancelled", xhr.data.error, "error");
                else
                    swal("Cancelled", xhr.statusText, "error");
            }
        })
    });

};
function handleAction($http, ActionUrl, dataJson, blockUIService, successCallback, forgeryTokenID) {
    ajaxPost($http, ActionUrl, dataJson, blockUIService, successCallback, null, forgeryTokenID);

};
function ShowSweetAlert(text, type) {
    /*
    success
    error
    warning
    */
    swal({
        title: text,
        type: type,
        html: true
    });

}
var resetInnerForm = function (data) {
    for (var prop in data) {
        if (data.hasOwnProperty(prop)) {
            if (typeof data[prop] != "boolean") {
                data[prop] = '';
            }
        }
    }
};

function MySubmitController($scope, $http, blockUI, $templateCache, $state, $templateCache, toaster) {
    $templateCache.put("select2/choices.tpl.html", "<ul tabindex=\"-1\" class=\"ui-select-choices ui-select-choices-content select2-results\"><li class=\"ui-select-choices-group\" ng-class=\"{\'select2-result-with-children\': $select.choiceGrouped($group) }\"><div ng-show=\"$select.choiceGrouped($group)\" class=\"ui-select-choices-group-label select2-result-label\" ng-bind=\"$group.name\"></div><ul id=\"ui-select-choices-{{ $select.generatedId }}\" ng-class=\"{\'select2-result-sub\': $select.choiceGrouped($group), \'select2-result-single\': !$select.choiceGrouped($group) }\"><li role=\"option\" ng-attr-id=\"ui-select-choices-row-{{ $select.generatedId }}-{{$index}}\" class=\"ui-select-choices-row\" ng-class=\"{\'select2-highlighted\': $select.isActive(this), \'select2-disabled\': $select.isDisabled(this)}\"><div class=\"select2-result-label ui-select-choices-row-inner\"></div></li></ul></li></ul>");
    $templateCache.put("select2/match-multiple.tpl.html", "<span class=\"ui-select-match\"><li class=\"ui-select-match-item select2-search-choice\" ng-repeat=\"$item in $select.selected track by $index\" ng-class=\"{\'select2-search-choice-focus\':$selectMultiple.activeMatchIndex === $index, \'select2-locked\':$select.isLocked(this, $index)}\" ui-select-sort=\"$select.selected\"><span uis-transclude-append=\"\"></span> <a href=\"javascript:;\" class=\"ui-select-match-close select2-search-choice-close\" ng-click=\"$selectMultiple.removeChoice($index)\" tabindex=\"-1\"></a></li></span>");
    $templateCache.put("select2/match.tpl.html", "<a class=\"select2-choice ui-select-match\" ng-class=\"{\'select2-default\': $select.isEmpty()}\" ng-click=\"$select.toggle($event)\" aria-label=\"{{ $select.baseTitle }} select\"><span ng-show=\"$select.isEmpty()\" class=\"select2-chosen\">{{$select.placeholder}}</span> <span ng-hide=\"$select.isEmpty()\" class=\"select2-chosen\" ng-transclude=\"\"></span> <abbr ng-if=\"$select.allowClear && !$select.isEmpty()\" class=\"select2-search-choice-close\" ng-click=\"$select.clear($event)\"></abbr> <span class=\"select2-arrow ui-select-toggle\"><b></b></span></a>");
    $templateCache.put("select2/no-choice.tpl.html", "<div class=\"ui-select-no-choice dropdown\" ng-show=\"$select.items.length == 0\"><div class=\"dropdown-content\"><div data-selectable=\"\" ng-transclude=\"\"></div></div></div>");
    $templateCache.put("select2/select-multiple.tpl.html", "<div class=\"ui-select-container ui-select-multiple select2 select2-container select2-container-multi\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled}\"><ul class=\"select2-choices\"><span class=\"ui-select-match\"></span><li class=\"select2-search-field\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" aria-activedescendant=\"ui-select-choices-row-{{ $select.generatedId }}-{{ $select.activeIndex }}\" class=\"select2-input ui-select-search\" placeholder=\"{{$selectMultiple.getPlaceholder()}}\" ng-disabled=\"$select.disabled\" ng-hide=\"$select.disabled\" ng-model=\"$select.search\" ng-click=\"$select.activate()\" style=\"width: 34px;\" ondrop=\"return false;\"></li></ul><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open || $select.items.length === 0}\"><div class=\"ui-select-choices\"></div></div></div>");
    $templateCache.put("select2/select.tpl.html", "<div class=\"ui-select-container select2 select2-container\" ng-class=\"{\'select2-container-active select2-dropdown-open open\': $select.open, \'select2-container-disabled\': $select.disabled, \'select2-container-active\': $select.focus, \'select2-allowclear\': $select.allowClear && !$select.isEmpty()}\"><div class=\"ui-select-match\"></div><div class=\"ui-select-dropdown select2-drop select2-with-searchbox select2-drop-active\" ng-class=\"{\'select2-display-none\': !$select.open}\"><div class=\"search-container\" ng-class=\"{\'ui-select-search-hidden\':!$select.searchEnabled, \'select2-search\':$select.searchEnabled}\"><input type=\"search\" autocomplete=\"off\" autocorrect=\"off\" autocapitalize=\"off\" spellcheck=\"false\" ng-class=\"{\'select2-active\': $select.refreshing}\" role=\"combobox\" aria-expanded=\"true\" aria-owns=\"ui-select-choices-{{ $select.generatedId }}\" aria-label=\"{{ $select.baseTitle }}\" class=\"ui-select-search select2-input\" ng-model=\"$select.search\"></div><div class=\"ui-select-choices\"></div><div class=\"ui-select-no-choice\"></div></div></div>");
    $scope.model = CurrentModel;
    $scope.submitForm = function (event, ActionUrl, FormID, refreshState, enableFormValidation) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, enableFormValidation, function successCallback(response) {
            blockUI.stop();
            ShowSweetAlert(response.data.Data, 'success');
            if (refreshState != null) {
                if (refreshState == 1) {
                    $state.reload();
                    $templateCache.removeAll();
                }
            }
        })
    };
    $scope.SignIn = function (event, ActionUrl, FormID) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            blockUI.stop();
            if (response.data.Type == "Success") {
                if (response.data.Message == "1") {
                    $('#NoLoginHeaderPanel').hide();
                    $('#LoginHeaderPanel').show();
                    $("#UsernameLbl").text(response.data.Object.Name + ' ' + response.data.Object.Family);
                    $state.go('UserProfile.Home');

                    //setTimeout(function () { $state.go('UserProfile.Home'); }, 3000);
                }
            }
            if (response.data.Type == "Error") {
                ShowSweetAlert(response.data.Data[0].errors.join(','), 'error')
            }
        })
    }
    $scope.ResetPassword = function (event, ActionUrl, FormID) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            blockUI.stop();
            if (response.data.Type === "Success") {
                ShowSweetAlert(response.data.Data, 'success')
            }
            if (response.data.Type === "Error") {
                ShowSweetAlert(response.data.Data[0].errors.join(','), 'error')
            }

        })
    }
    /*************************** Csmi ***************************/
    $scope.SendArticle = function (event, ActionUrl, FormID) {
        event.preventDefault();
        if (selectedIDs != undefined) {
            if (selectedIDs.split(',').length > 0) {
                $scope.model.selectedArticleIDs = selectedIDs.split(',');
                var thisForm = $('#' + FormID);
                SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
                    blockUI.stop();
                    if (response.data.Type == "Success") {
                        ArticleManagementGrid.PerformCallback();
                        $('#myModal2').modal('hide');
                        ShowSweetAlert('با موفقیت ثبت شد', 'success');

                    }
                    else {

                        ShowSweetAlert('خطا در ثبت', 'error');
                    }
                })
            }
            else {
                ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
            }
        }
        else {
            ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
        }

    }
    $scope.searchCity = function () {
        blockUI.start();
        return $http.post(feedUrl, {
            ProvinceID: $scope.model.selectedProvinceID
        }).then(function (response) {
            $scope.model.CityFeeds = response.data;
            $scope.model.selectedCityID = $scope.model.CityFeeds[0].Value;
            blockUI.stop();
        });
    }
    $scope.getTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.model.ConferencePackages.length; i++) {
            var product = $scope.model.ConferencePackages[i];
            total += (product.Price * product.Count);
        }
        total += $scope.model.CompanionPackage.Price;
        return total;
    }
    $scope.getCompanyTotal = function () {
        var total = 0;
        for (var i = 0; i < $scope.model.OtherPackages.length; i++) {
            var product = $scope.model.OtherPackages[i];
            total += (product.Price * product.Count);
        }
        total += ($scope.model.RegisterPackages[$scope.model.SelectedRegisterPackageIndex].Price * $scope.model.SelectedRegisterPackageCount);
        return total;
    }
    $scope.submitPayForm = function (event, ActionUrl, FormID, refreshState) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            blockUI.stop();
            $state.go('UserProfile.Pay', { OrderID: response.data.Data, Type: 'Order' });
        })
    };
    $scope.submitPayVipForm = function (event, ActionUrl, FormID, refreshState) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            blockUI.stop();
            $state.go('UserProfile.Pay', { OrderID: response.data.Data, Type: 'SocietyVipOrder' });
        })
    };
    $scope.submitOnlinePayForm = function (event, ActionUrl, FormID, refreshState) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            $scope.model = response.data;
            setTimeout(function () { blockUI.stop(); $("#OnlinePayform").submit() }, 1500)
            //$("#OnlinePayform").submit();
        })
    };

    $scope.IsChangePersonalPic = true;
    $scope.changePersonalPic = function () {
        $scope.$apply(function (scope) {
            scope.IsChangePersonalPic = false;
        });
    };
    $scope.IsChangeMeliCardPic = true;
    $scope.changeMeliCardPic = function () {
        $scope.$apply(function (scope) {
            scope.IsChangeMeliCardPic = false;
        });
    };
    $scope.VoteCheckChange = function (candidate) {
        $scope.model.candidCnt = 0;
        $scope.model.inspectorCnt = 0;
        for (i = 0; i < $scope.model.UserVotes.length; i++) {
            if ($scope.model.UserVotes[i].IsChecked) {
                if ($scope.model.UserVotes[i].Candidate.SelectedCandidateTypeID == 3) {
                    $scope.model.candidCnt++;
                }
                if ($scope.model.UserVotes[i].Candidate.SelectedCandidateTypeID == 4) {
                    $scope.model.inspectorCnt++;
                }
            }
        }
        if ($scope.model.candidCnt >= 6) {
            candidate.IsChecked = false;
            $scope.model.candidCnt--;
            ShowToast(toaster, 'شما فقط می توانید ۵ نفر کاندید هیات مدیره را انتخاب کنید', 'error')
        }
        if ($scope.model.inspectorCnt >= 2) {
            candidate.IsChecked = false;
            $scope.model.inspectorCnt--;
            ShowToast(toaster, 'شما فقط می توانید ۱ نفر کاندید بازرس را انتخاب کنید', 'error');
        }
    }
    $scope.SendActivationCode = function (event, ActionUrl) {
        event.preventDefault();
        blockUI.start();
        handleAction($http, ActionUrl, null, blockUI, function successCallback(response) {
            blockUI.stop();
            ShowToast(toaster, response.data.Data, 'success')
        }, 'forgeryToken');
    }
    //$scope.CheckMeliCode = function ($event, ActionUrl, MeliCode) {
    //    event.preventDefault();
    //    blockUI.start();
    //    ajaxPost($http, ActionUrl, { MeliCode: MeliCode }, toaster, blockUI, function (response) {
    //        $scope.model[PersonVariableName] = response.data;
    //        $scope.model[PersonVariableName].Melicode = MeliCode;
    //        blockUI.stop();
    //    });


    //}
    $scope.AddTempWriter = function (event, formIsValid) {
        event.preventDefault();
        $scope.submitted = true;
        if (formIsValid) {
            var copiedObject = jQuery.extend({}, $scope.model.Writer);
            $scope.model.Writers.push(copiedObject);
            ShowToast(toaster, 'نویسنده با موفقیت اضافه شد', 'success');
            $('#smartwizard').children("div").finish().animate({ height: $(CurrentStep).outerHeight() + 100 }, '400', function () { })
            resetInnerForm($scope.model.Writer);
            $scope.model.Writer.ID = 0;
            $scope.submitted = false;
        }
    }
    $scope.RemoveTempWriter = function (event, index) {
        event.preventDefault();
        if (confirm('آیا از حذف اطمینان دارید؟')) {
            $scope.model.Writers.splice(index, 1);
            $('#smartwizard').children("div").finish().animate({ height: $(CurrentStep).outerHeight() }, '400', function () { })
        }
    }
}
function MyGridController($scope, $http, $state, blockUI, blockUI, $templateCache, $uibModal) {
    $templateCache.removeAll();
    $scope.selectedIDs = selectedIDs;
    $scope.DeleteAction = function (DeleteUrl, Grid) {
        if (selectedIDs != undefined) {
            if (selectedIDs.split(',').length > 0) {
                $scope.selectedIDs = selectedIDs.split(',');
                var data = new Object();
                data.ids = selectedIDs.split(',');
                handleDeleteAction($http, DeleteUrl, data, eval(Grid))
            }

            else {
                ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
            }
        }
        else {
            ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
        }
    };
    $scope.EditAction = function (state) {
        if (selectedIDs != undefined) {
            if (selectedIDs.split(',').length > 0) {
                $state.go(state, { id: selectedIDs.split(',')[0] });
            }
            else {
                ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
            }
        }
        else {
            ShowSweetAlert('لطفا یک مورد را انتخاب نمایید', 'error')
        }
    }
    $scope.ActiveAction = function (ActionUrl, Grid) {
        if (selectedIDs != undefined) {
            if (selectedIDs.split(',').length > 0) {
                var data = new Object();
                data.ids = selectedIDs.split(',');
                handleAction($http, ActionUrl, data, blockUI, function successCallback(response) {
                    ShowSweetAlert(response.data.Data, 'success');
                    eval(Grid).Refresh();
                }, 'forgeryToken');
            }
            else {
                ShowToast(toaster, 'لطفا یک مورد را انتخاب نمایید', 'error')
            }
        }
        else {
            ShowToast(toaster, 'لطفا یک مورد را انتخاب نمایید', 'error')
        }
    }
    $scope.GotoState = function (state, params) {
        if (params != undefined && params != null)
            $state.go(state, params);
        else
            $state.go(state);
    }
    /*************************** Csmi ***************************/

}
function MyLayoutController($scope, $http, blockUI, $templateCache, $state) {
    $scope.SignOut = function (ActionUrl, logOutUrl) {
        handleAction($http, ActionUrl, [], blockUI, function successCallback(response) {
            if (response.data.Data == '1') {
                window.location = logOutUrl;
            }
        }, 'forgeryTokenLogOut');
    }
}
function ListController($scope, $http, $state, blockUI, $templateCache) {
    $scope.model = ListModel;
    $scope.PageChangeActionUrl = PageChangeActionUrl;
    $scope.DataModel = ListModel[ListName];
    $scope.totalRecords = ListModel.totalRecords;
    $scope.pageSize = ListModel.pageSize;// this should match however many results your API puts on one page  
    $scope.pagination = {
        current: 1
    };
    $scope.pageChanged = function (newPage) {
        getResultsPage(newPage);
    };

    function getResultsPage(pageNumber) {
        // this is just an example, in reality this stuff should be in a service
        blockUI.start();
        $http.get($scope.PageChangeActionUrl + '?pageNumber=' + pageNumber)
            .then(function (result) {
                blockUI.stop();
                $scope.DataModel = result.data[ListName];
                $scope.totalRecords = result.data.totalRecords
            });
    }

    $scope.DeleteAction = function (DeleteUrl, ID) {
        var data = new Object();
        data.ids = ID;
        handleDeleteAction($http, DeleteUrl, data, $state, $templateCache)
    };
    $scope.GotoState = function (state, params) {
        if (params != undefined && params != null)
            $state.go(state, params);
        else
            $state.go(state);
    }

    /**************************News*********************************/
    $scope.NewsPageChanged = function (pageNumber) {
        getNewsResultsPage(pageNumber, $state.params.year);
    };
    function getNewsResultsPage(pageNumber, year) {
        // this is just an example, in reality this stuff should be in a service
        blockUI.start();
        $http.get($scope.PageChangeActionUrl + '?pageNumber=' + pageNumber + '&year=' + year)
            .then(function (result) {
                blockUI.stop();
                $scope.DataModel = result.data[ListName];
                $scope.totalRecords = result.data.totalRecords
            });
    }
    /**************************Book*********************************/
    $scope.BookPageChanged = function (pageNumber) {
        getBookResultsPage(pageNumber, $scope.model.Writer);
    };
    function getBookResultsPage(pageNumber, WriterName) {
        // this is just an example, in reality this stuff should be in a service
        blockUI.start();
        $http.get($scope.PageChangeActionUrl + '?pageNumber=' + pageNumber + '&WriterName=' + WriterName + '&OrderType=' + $scope.model.OrderType)
            .then(function (result) {
                blockUI.stop();
                $scope.DataModel = result.data[ListName];
                $scope.totalRecords = result.data.totalRecords
            });
    }
    /**************************Articles*********************************/
    $scope.ArticlePageChanged = function (pageNumber) {
        getArticlesResultsPage(pageNumber);
    };
    function getArticlesResultsPage(pageNumber) {
        // this is just an example, in reality this stuff should be in a service
        blockUI.start();
        var copiedObject = jQuery.extend({}, $scope.model);
        copiedObject.pageNumber = pageNumber;
        ajaxPost($http, $scope.PageChangeActionUrl, copiedObject, blockUI, function (result) {
            blockUI.stop();
            $scope.model = result.data;
            $scope.DataModel = result.data[ListName];
            $scope.totalRecords = result.data.totalRecords;
        });
    }
    $scope.ArticleLikeInit = function (Article) {
        var selectedArticle = $scope.DataModel.find(obj => { return obj.ID === Article.ID });
        if (getCookie('Article' + Article.ID) != "") {
            selectedArticle.AllowLike = false;
        }
        else {
            selectedArticle.AllowLike = true;
        }
    }
    $scope.ArticleLikeChange = function (ActionUrl, id) {
        ajaxPost($http, ActionUrl, { "ArticleID": id }, blockUI, function (response) {
            blockUI.stop();
            if (response.data.Type == "Success") {
                ShowSweetAlert('با موفقیت ثبت شد', 'success');
                if (getCookie('Article' + id) == "") {
                    setCookie('Article' + id, 1, 7)
                    var selectedArticle = $scope.DataModel.find(obj => { return obj.ID === id });
                    selectedArticle.LikeCnt++;
                    selectedArticle.AllowLike = false;
                }
                else
                    ShowSweetAlert('شما قبلا نظر خود را درباره این مقاله ثبت کرده اید', 'error');
            }
            else {
                ShowSweetAlert('خطا در ثبت', 'error');
            }
        });
    }
    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
    }

    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }
}
function MasterPageSubmitController($scope, $http, blockUI, $templateCache, $state, $templateCache) {
    $scope.model = MasterPageCurrentModel;
    $scope.submitForm = function (event, ActionUrl, FormID) {
        event.preventDefault();
        var thisForm = $('#' + FormID);
        SubmitForm(ActionUrl, thisForm, $scope.model, $http, blockUI, true, function successCallback(response) {
            blockUI.stop();
            ShowSweetAlert(response.data.Data, 'success')
        })
    }
    $scope.EditProfile = function () {

        $state.go('UserProfile.Profile');
        $('#ms-account-modal').modal('hide');
    }
    $scope.SignOut = function (ActionUrl, logOutUrl) {
        SignOut(ActionUrl, logOutUrl, $http, blockUI, $templateCache, $state);
        $('#ms-account-modal').modal('hide');
    }
}
function CleanCache($scope, $templateCache, $state) {
    $templateCache.removeAll();
}
/**
 *
 * Pass all functions into module
 */
angular
    .module('CSMI')
    .controller('MainCtrl', ['$http', 'blockUI', MainCtrl])
    .controller('passwordMeterCtrl', passwordMeterCtrl)
    .controller('translateCtrl', ['$translate', '$scope', '$state', '$http', 'blockUI', translateCtrl])
    .controller('MySubmitController', ['$scope', '$http', 'blockUI', '$templateCache', '$state', '$templateCache', 'toaster', MySubmitController])
    .controller('MasterPageSubmitController', ['$scope', '$http', 'blockUI', '$templateCache', '$state', '$templateCache', MasterPageSubmitController])
    .controller('MyLayoutController', ['$scope', '$http', 'blockUI', '$templateCache', '$state', MyLayoutController])
    .controller('ListController', ['$scope', '$http', '$state', 'blockUI', '$templateCache', ListController])
    .controller('CleanCache', ['$scope', '$templateCache', '$state', CleanCache])

