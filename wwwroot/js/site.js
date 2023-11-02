

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


