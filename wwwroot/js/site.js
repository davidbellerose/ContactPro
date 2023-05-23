

// **************************************************
//
//              BACK TO TOP BUTTON
//
//      move above footer when scrolled to bottom
//
//
// **************************************************


$(document).ready(function () {

    $(document).on('scroll', function () {
        var distanceFromBottom = $(document).height() - ($(document).scrollTop() + $(window).height());

        if (distanceFromBottom < 80) {
            $('#back-top').addClass("shift");
        } else {
            $('#back-top').removeClass("shift");
        }
    });
});


// fade in and out the back to top button
$(window).scroll(function () {
    if ($(document).scrollTop() > 1) {
        $('#back-top').fadeIn(800);
    } else {
        $('#back-top').fadeOut(400);
    }

});



function formatPhone() {

    let phone = document.getElementById('phoneNumber');
    let str = phone.value;
    let span = document.getElementById('phoneValidation')

    //let createBtn = document.getElementById("createBtn");
    let saveBtn = document.getElementById("saveBtn");

    var numberPattern = /\d+/g;
    str = str.match(numberPattern).join('');

    if (str.length != 10) {
        span.innerHTML = "The phone number must be 10 numbers.";
        saveBtn.disabled = true;
        //createBtn.disabled = true;
    } else {
        span.innerHTML = "";
        //Filter only numbers from the input
        let cleaned = ('' + str).replace(/\D/g, '');

        //Check if the input is of correct length
        let match = cleaned.match(/^(\d{3})(\d{3})(\d{4})$/);

        if (match) {
            str = '(' + match[1] + ') ' + match[2] + '-' + match[3];
            phone.value = str;
        };
        alert(str);
        saveBtn.disabled = false;
        //createBtn.disabled = false;
    }
    return null
}


// tooltip for crud actions
//document.addEventListener("mouseover", (event) => { });

//function tipEdit(){
//    let balloon = document.getElementsByClassName("balloon");
//    balloon.innerHTML = "Edit Contact";
//}
//function tipEmail() {
//    let balloon = document.getElementsByClassName("balloon");
//    balloon.innerHTML = "Email Contact";
//}
//function tipDelete() {
//    let balloon = document.getElementsByClassName("balloon");
//    balloon.innerHTML = "Delete Contact";
//}

//function tipNill() {
//    let balloon = document.getElementsByClassName("balloon");
//    balloon.innerHTML = "";
//}


// **************************************************
//
//              COOKIE MODAL
//
// **************************************************
function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

// Setup GTM dataLayer -- load
window.dataLayer = window.dataLayer || [];

// see if cookie exists, if not show cookiebar -- recommended to do this in head of page
if (!getCookie("CookieConsent")) {

    document.getElementsByClassName("js-cookiebar")[0].style.display = "block";

    window.dataLayer = [{
        'event': 'NoCookieExist'
    }];
} else {
    var cookieValue =
        "{" + '"cookieValue"' + ":" + getCookie("CookieConsent") + "}";
    var jsonData = JSON.parse(cookieValue);
    pushCookieValueToDataLayer(jsonData.cookieValue, "CookieBarStatus", false);
}

// get Cookie by name - credits to stackoverflow
function getCookie(cname) {
    var name = cname + "=",
        decodedCookie = decodeURIComponent(document.cookie),
        ca = decodedCookie.split(";");

    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == " ") {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// push cookieValue to dataLayer - set array, set eventName
function pushCookieValueToDataLayer(arr, eventName, boolean) {
    var x = arr.filter(function (v) {
        return v.value === true;
    });
    dataLayerType(x, eventName, boolean);
}

function dataLayerType(x, e, b) {
    if (b) {
        for (var i = 0; i < x.length; i++) {
            window.dataLayer.push(_defineProperty({}, x[i].name, x[i].value));
        }
        window.dataLayer.push({
            'event': e
        });
    } else {
        var dataInit = [];
        for (var i = 0; i < x.length; i++) {
            dataInit.push(_defineProperty({}, x[i].name, x[i].value));
        };
        dataInit.push({
            'event': e
        });
        window.dataLayer = dataInit;
    }
}

// set cookie on click
function onCookieButtonClick() {
    var basicValue = document.getElementById("basic_chkbx").checked,
        preferencesValue = document.getElementById("preferences_chkbx").checked,
        statisticsValue = document.getElementById("statistics_chkbx").checked,
        marketingValue = document.getElementById("marketing_chkbx").checked,
        options = [
            { name: "basic", value: basicValue },
            { name: "preferences", value: preferencesValue },
            { name: "statistics", value: statisticsValue },
            { name: "marketing", value: marketingValue }
        ],
        data = JSON.stringify(options),
        date = new Date();

    date.setTime(date.getTime() + 3650 * 24 * 60 * 60 * 1000);
    var expires = "expires=" + date.toUTCString();

    // set cookie
    document.cookie = "CookieConsent=" + data + ";" + expires + ";";
    pushCookieValueToDataLayer(options, "CookiePreferencesChange", true);

    document.getElementsByClassName("js-cookiebar")[0].style.display = "none";
}

// show cookiebar on click
var changeCookie = document.getElementsByClassName("js-change-cookie")[0];

// Get cookievalue, set checkboxes to right value and show cookiebar
changeCookie.addEventListener(
    "click",
    function () {
        if (getCookie("CookieConsent")) {
            var cookieValue =
                "{" + '"cookieValue"' + ":" + getCookie("CookieConsent") + "}";
            var jsonData = JSON.parse(cookieValue);
            var x = jsonData.cookieValue.filter(function (v) {
                return v.value === true;
            });

            for (let i = 0; i < x.length; i++) {
                let y = x[i].name + "_chkbx";
                document.getElementById(y).checked = true;
            }
        }

        document.getElementsByClassName("js-cookiebar")[0].style.display = "block";
    },
    false
);


// **************************************************
//
//              PAGE LOADER
//
// **************************************************

//document.onreadystatechange = function () {
//    if (document.readyState !== "complete") {
//        alert("loading");
//        //const loadText = document.querySelector(".loading-text");
//        //const bg = document.querySelector(".loader");
//        //alert(loadText);
//        //alert(bg);

//        //let load = 0;

//        //let interval = setInterval(blurring, 30);

//        //function blurring() {
//        //    load++;
//        //    if (load > 99) {
//        //        clearInterval(interval);
//        //    }
//        //    loadText.innerText = `${load}%`;
//        //    loadText.style.opacity = 1 - load / 100;
//        //    loadText.style.width = `${10 + load}%`;
//        //    bg.style.filter = `blur(${30 - (load * 30) / 100}px)`;

//        document.getElementById(
//            "body").style.visibility = "hidden";
//        document.getElementById(
//            "loading-text").style.visibility = "visible";

//    } else {
//        alert("complete");
//        document.getElementById(
//            "loading-text").style.display = "none";
//        document.getElementById(
//            "body").style.visibility = "visible";
//    }
//};