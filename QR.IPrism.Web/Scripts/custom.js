var vpwM = $(window).width();
function checkIsMobile() {
    //$('#logininfoMenutoggle').tabCollapse();
   
    if (vpwM > 769) {
        //appSettings.isMobile = false;
        $(".actvDate h1").removeClass("roster_Datah11");
        //$('.submenuicon').show();
        //$('.profile').show();
        $("#navbar").collapse('hide');

    } else {
        appSettings.isMobile = true;
        //$('.submenuicon').hide();
        //$('.profile').hide();
    }
}


function onClickRemoveMenu() {
    $(".icon-toggle.collapse.in").removeClass("in");
    $('body').removeClass("padding-top-small");
}

function onClickMenu() {
   // $('#logininfoMenutoggle').tabCollapse();
    //$('#menutoggleButton').click();
    //$('#logininfoMenutoggle').click();  $("#logininfoMenutoggle").collapse('show'); 
   
        if ($('#isMenuSelected').val() == '0') {
            // $('#logininfoMenutoggle').width(10);
            //$('#logininfoMenutoggle').show();
            //$('.submenuicon').show();
            //$('.profile').show();

           $('#logininfoMenutoggle').addClass('in');
            $('#isMenuSelected').val('1');
        } else {
            //$('#logininfoMenutoggle').hide();
            // $('#logininfoMenutoggle').width(0);
            //$('.submenuicon').hide();
            //$('.profile').hide();

            $('#logininfoMenutoggle').removeClass('in');
            $('#isMenuSelected').val('0');

            //$('body').removeClass("padding-top-large");
            //$('body').removeClass("padding-top-small");
        }
}

function onClickMainMenu() {
    if (vpwM > 769) {
        //$('.btn-navbar').click(); //bootstrap 2.x
        //$('.navbar-toggle').click() //bootstrap 3.x by Richard
        //$('#mainNavbarButton').click(); //bootstrap 2.x
        //$('#navbar').click(); //bootstrap 3.x by Richard
        $("#navbar").collapse('hide');
        $('body').removeClass("padding-top-large");
        $('body').removeClass("padding-top-small");
    }


}
$(document).ready(function () {
    //$('body .container-fluid').click(function (event) {
    //    event.stopPropagation();
    //    alert('aaaa');
    //    $('.collapse').collapse('hide'); $('body').removeClass("padding-top-small"); $('body').removeClass("padding-top-large");
    //});
  
    resizeDiv();
    setColumnHeight();
    setContainerHeight();
    checkIsMobile();
    
    //$("#homeUIViewDiv container-fluid").click(function () {
    //    $(".icon-toggle.collapse.in").removeClass("in");
    //    $('body').removeClass("padding-top-small");
    //});
/*TO make the Roaster Trip Details Open on page load for Desktop web version*/

    var vpw = $(window).width();

    if (vpw > 768) {
    $('.defaultActive').click();
    }

 /* -------------CLOSE----------------*/

    $('.weekRoster').removeClass('Roster_Monthly_new');

    $(document).on('show', '.accordion', function (e) {
        $(e.target).prev('.accordion-heading').addClass('accordion-opened');
    });

    $(document).on('hide', '.accordion', function (e) {
        $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
    });

    $(window).scroll(function () {

        if ($(this).scrollTop() > 100) {
            $('.scrollupWindow').fadeIn();
        } else {
            $('.scrollupWindow').fadeOut();
        }
    });
    $('.scrollupWindow').click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, 600);
        return false;
    });

    $(".rosterList").scroll(function () {

        if ($(this).scrollTop() > 100) {
            $('.scrollup').fadeIn();
        } else {
            $('.scrollup').fadeOut();
        }
    });

    $('.scrollup').click(function () {
        $(".rosterList").animate({
            scrollTop: 0
        }, 600);
        return false;
    });

    $("#latestUpdates_mobile").hide();
    $("#svp_mobile").hide();

});



/*-------Left & Right Arrow on Weekly Roaster Movement----------*/

$(document).ready(function(){
    var widthOneItem = parseInt($(".roster-ListItem").first().css("width"))+parseInt($(".roster-ListItem").first().css("margin-left"))
    parseInt($(".roster-ListItem").first().css("margin-right"))+parseInt($(".roster-ListItem").first().css("padding-left"))+
    parseInt($(".roster-ListItem").first().css("padding-right"));
    $(".arrow-left").click(function(){
        $(".pg-cnt").animate({scrollLeft: "-="+widthOneItem});
    });
    $(".arrow-right").click(function(){
        $(".pg-cnt").animate({scrollLeft: "+="+widthOneItem});
    });        
});

/*------------------------CLOSE----------------------------------------*/


/*----------Header Menu, Profile, Alert menu hover and effects--------*/

$(document).ready(function () {
 
        $(".alertbell").click(function (e) {
        $(".alertbellpopup").slideToggle(700);
        $('.profilepicpopup').hide();
                    
        });
    
        $(".profilePic").click(function (e) {
        $(".profilepicpopup").slideToggle(700);
         $('.alertbellpopup').hide();
            
        });

       $(document).click(function(e){
        if($(e.target).closest('.alertbellpopup').length != 0) return false;
        $('.alertbellpopup').hide();

        });

        $(document).click(function(e){
        if($(e.target).closest('.profilepicpopup').length != 0) return false;
        $('.profilepicpopup').hide();
        });

        $(document).click(function(e){
        if($(e.target).closest('.caret').length != 0) return false;
             $('.caret').addClass("caretcolordark").removeClass("caretcolorwhite");
        });

        $(".menu-part li").on('click',function(e){
            $(this).children("a").children(".caret").addClass("caretcolorwhite").removeClass("caretcolordark");
            $(this).siblings().children("a").children(".caret").addClass("caretcolordark").removeClass("caretcolorwhite");
        });
        $(".menu-part li").on(' mouseover',function(e){
            $(this).children("a").children(".caret").addClass("white").removeClass("dark");

        });
        $(".menu-part li").on('mouseout',function(e){
            $(this).children("a").children(".caret").addClass("dark").removeClass("white");

        });
      
        $(".navbar-header .dot-toggle").on('click',function(){
             $("body").toggleClass("padding-top-small");
            if($(".main-menu-nav.collapse.in").length>0){
                $(".main-menu-nav.collapse.in").removeClass("in");
               $('body').removeClass("padding-top-large");
             }
        });
        
        $(".navbar-header .main-toggle").on('click',function(mainDropdownheight){
             $("body").toggleClass("padding-top-large");
    
              if($(".icon-toggle.collapse.in").length>0){
                $(".icon-toggle.collapse.in").removeClass("in");
               $('body').removeClass("padding-top-small");
             }

        });     
       
   });

/*----------------------CLOSE----------------------------*/

  


$(function () {

    $('.accordions_cabin h4:first').addClass('active').next().slideDown('slow');
    $('.accordions_cabin h4').click(function () {

        $('.accordions_cabin h4').removeClass('active');
        $(this).addClass('active').next().slideDown('slow');

    });


    $('.accordions_link h4:first').addClass('active').next().slideDown('slow');
    $('.accordions_link h4').click(function () {

        $('.accordions_link h4').removeClass('active');
        $(this).addClass('active').next().slideDown('slow');

    });
});



//$(".rooster-day").click(function (event) {

//    event.preventDefault();
     
//    $(".roster_Data.actvDate").removeClass("actvDate");
//    $(".rooster-day").removeClass("actvRosterEvent");

///*---------------For Roaster Cell Stylings----------------------------*/
//    $(".rooster-day.activeDate").removeClass("activeDate");
//    $(".roster_Data.rosteractive").removeClass("rosteractive");
//    $(".rooster-day.rosteractive").removeClass("rosteractive");
//    $(".roster-ListItem.no-Border").removeClass("no-Border");

//    $(this).siblings(".rooster-day").addClass("activeDate");
//    $(this).closest(".roster_Data").addClass("rosteractive");
//    $(this).parents(".roster-ListItem").addClass("no-Border");

///*--------------------------CLOSE-----------------------------------------*/

//    $(this).addClass("actvRosterEvent");
//    $(this).closest("li").addClass("actvDate");

//    $('#roster_details').hide();
//    $('#back_calendar').show();
//    $('#roster_details').slideDown("slow", function () {
//        // Animation complete.
//    });
//    $(".pageTitle").addClass("mobile_pageTitle");
//    $(".roster-List").animate({ scrollTop: 0 }, 600);


//});


//back calendar
$("#back_calendar").click(function (event) {
    $(".roster_Data").removeClass("DeviceRosterData");
    $(".rooster-day.actvRosterEvent").removeClass("actvRosterEvent");
    $("#roster_details").hide();
    $("#back_calendar").hide();
    $(".pageTitle").removeClass("mobile_pageTitle");
    document.getElementById('monthDate').innerHTML = 'OCT 2015';
   
 /* --------------------Roaster Cell Styling -----------------  */
    $(".roster_Data.rosteractive").removeClass("rosteractive");
     $(".roster-ListItem.no-Border").removeClass("no-Border");
     $(".rooster-day.activeDate").removeClass("activeDate");

    /* ---------------------CLOSE--------------------------------*/


});

//close tab contet
$(".closeTabContent").click(function (event) {
    $('#roster_details').fadeOut();
    $(".rooster-day.actvRosterEvent").removeClass("actvRosterEvent");

   /* --------------Roaster Trip Details Close Button functionality -------------------------*/
    $(".roster_Data.rosteractive").removeClass("rosteractive");
    $(".rooster-day.rosteractive").removeClass("rosteractive");
    $(".roster-ListItem.no-Border").removeClass("no-Border");
    $(".rooster-day.activeDate").removeClass("activeDate");

   /* -----------------------CLOSE-----------------------------------*/

});


$(document).ready(function(){
//menu clicks
    $(".showAlerts").click(function (event) {
    //mobile
            hideContent();
    $('#svp_mobile').hide();
    //show
    $("#latestUpdates_mobile").show();
    });

    $(".showSVP").click(function (event) {
    //mobile
          hideContent();
    $('#svp_mobile').show();
    });



    $(".myroster_mobile").click(function (event) {
    //mobile
         hideContent();
    $('#svp_mobile').hide();
    //show
    $('#myroster').show();
    });


    
    $(".showDoculibry").click(function (event) {
    //mobile
         hideContent();
    $('#svp_mobile').hide();
    //show
    $('#document_lib').show();
  
    });
    $(".showLinks").click(function (event) {
         //mobile
         hideContent();
        $('#svp_mobile').hide();
        //show
        $('#useful_links').show();
    });


/*---Hiding content Alerts,SVP,Document Library  in mobile-----*/
    function hideContent(){
    $('#latestUpdates').hide();
    $('#Vision_News').hide();
    $('#document_lib').hide();
    $('#useful_links').hide();
    $('#myroster').hide();
    $("#latestUpdates_mobile").hide();
    $("#navbar").removeClass("in");
    $('body').removeClass("padding-top-large");
    }
});
/*-------------------------CLOSE---------------------------------*/


//#latestUpdates, #Vision_News, #document_lib, #useful_links

window.onresize = function (event) {
    resizeDiv();
    setColumnHeight();
    setContainerHeight();
    checkIsMobile();
}


function setColumnHeight() {
    vpw = $(window).width();
    if (vpw < 990){
         //$(".rooster-day").click(function() {
        //$(".rooster-day").removeClass("weekly_roster_details");
        //   $(this).addClass("weekly_roster_details");
        //  });
    }
    
    if (vpw < 769) {
        $(".FullRoster").removeClass("Roster_Monthly_new");
        $('.rosterList > ul > li').css('height', 'auto');
        $(".monthly_roster_new li").removeClass("month-fix");
        $('.roster-List > li').css('display', 'table-row');

/*-------------Mobile Alert & Notification Display -------------*/
    //$(".alert-bell-icon").click(function(){
    //    $("#myroster").hide();
    //    $("#latestUpdates_mobile").show();
    // });
/*  ----------------------CLOSE-----------------------------*/

/* ---------------Mobile on Every Roaster click redirection for Roaster trip Details-------*/

     //$(".rooster-day").click(function() {
     //   $(".pageTitle").hide();
     //   $("#mob-changeTimeZoneHide").hide();
     //   $("#weekmonthtab").hide();
     //   $("#roster_details").show();      
     //   $(".wk-myrosterLink").addClass("wk-myrosterLink1");
     //   $("#roster_details").addClass("roster_details1");
     //   $(".pg-cnt").addClass("pg-cnt1");
     //   $(".roosterDay-wrp .rooster-day").addClass("roosterDay-wrp1");
     //   $(".actvDate h1").addClass("roster_Datah11");
     //   $(".roster_Data h1 i.flight_date").addClass("roster_Datah1flightdate");
     //   $(".roster_Data h1 p").addClass("roster_Datah1flightdatep");
     //   $(".roosterDay-wrp .extra-day").addClass("extra-day-no-border");
     //   $(".roosterDay-wrp").addClass("pg-cnt2");
     //   $(".roster-ListItem").addClass("rosterlistitemdisplay");
     //   $(".roster_Data").parent().parent().addClass("DeviceRosterData");
     //   $('.roster_Data.actvDate ').addClass("actvDateMar")
     //   $('.roster-List > li > ul').addClass("activatedrosterlistul");
     //   $('.roster_Data.actvDate').addClass("activatedatermvshad");
     //   $('.weeklykDv').hide();
     //   $('.monthlyDv').hide();
     //   $('.dateSliderWeek').hide();
     //   });

   /* --------------------------CLOSE---------------------------------------*/


  /* -- Mobile on click in each Roaster, above roaster trip details triangle black effect to mark selected roaster ---*/
        //$(".rooster-day").click(function() {
        //$(".rooster-day").removeClass("weekly_roster_details");
        //$(this).addClass("weekly_roster_details");
        //});
    /* -----------------------------CLOSE-------------------------------------------*/

    }

    else if (vpw > 769) {
  
        $('#latestUpdates').show();
        $('#Vision_News').show();
        $('#document_lib').show();
        $('#useful_links').show();
        $('#myroster').show();
        $('#svp_mobile').hide();

        $(".FullRoster").addClass("Roster_Monthly_new");
        $('.rosterList').css({ 'height': 'auto' });
        $(".monthly_roster_new li").addClass("month-fix");
        $('.roster-List > li').css('display', 'table-cell');

   /*  --- Roaster cell width & spacing--------*/

        //$('.roster-List > li').css('margin', '5px');
        //$('.monthly_roster_new .roster-List > li').css('width','13%');

   /* ------------------CLOSE--------------------------*/


        boxes = $('.monthly_roster_new > ul > li');
        maxHeight = Math.max.apply(
             Math, boxes.map(function () {
                 return $(this).height();
             }).get());
        boxes.height(maxHeight);
    }

}


function resizeDiv() {
    vph = $(window).height() - 180;

    vpw = $(window).width();

    if (vpw < 768) {

        //$('.rosterList').css({ 'height': vph + 'px' });
    }

    else if (vpw > 769) {
        $('.rosterList').css({ 'height': 'auto' });
              $('#latestUpdates').show();
              $('#Vision_News').show();
    }
    $('body').removeClass("padding-top-large");
    $('body').removeClass("padding-top-small");
}

function setContainerHeight() {
    var getHeight = $(window).height() - 150;
    //$('.content_wrapper').css({ 'min-height': getHeight + 'px' });
    $('.content_wrapper').css('min-height', getHeight + 'px');
}

window.addEventListener("orientationchange", function () {
    resizeDiv();
    setColumnHeight();
    checkIsMobile();

    var orientation = window.orientation;

    // Look at the value of window.orientation:

    if (orientation === 0) {

        // iPad is in Portrait mode
        $('.monthly_roster_new .roster-List > li').css('float', 'left');
        $('.monthly_roster_new .roster-List > li').css('display', 'inline-block');
    }

    else if (orientation === 90) {

        // iPad is in Landscape mode. The screen is turned to the left.
        $('.weekRoster .roster-List').css('display', 'table-row');
        $('.weekRoster .roster-List > li').css('display', 'table-cell');

        $('.weekRoster').removeClass('Roster_Monthly_new');

    }


    else if (orientation === -90) {

        // iPad is in Landscape mode. The screen is turned to the right.
        $('.weekRoster .roster-List').css('display', 'table-row');
        $('.weekRoster .roster-List > li').css('display', 'table-cell');

        $('.weekRoster').removeClass('Roster_Monthly_new');
    }

    else if (orientation === 180) {

        // iPad is in Portrait mode
        $('.monthly_roster_new .roster-List > li').css('float', 'left');
        $('.monthly_roster_new .roster-List > li').css('display', 'inline-block');
    }

}, false);

/*Chart JS*/

//$.getScript('http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js', function () {
//    $.getScript('http://cdnjs.cloudflare.com/ajax/libs/morris.js/0.5.0/morris.min.js', function () {

//        Morris.Area({
//            element: 'area-example',
//            data: [
//      { y: '1.1.', a: 100, b: 90 },
//      { y: '2.1.', a: 75, b: 65 },
//      { y: '3.1.', a: 50, b: 40 },
//      { y: '4.1.', a: 75, b: 65 },
//      { y: '5.1.', a: 50, b: 40 },
//      { y: '6.1.', a: 75, b: 65 },
//      { y: '7.1.', a: 100, b: 90 }
//    ],
//            xkey: 'y',
//            ykeys: ['a', 'b'],
//            labels: ['Series A', 'Series B']
//        });

//        Morris.Line({
//            element: 'line-example',
//            data: [
//          { year: '2010', value: 20 },
//          { year: '2011', value: 10 },
//          { year: '2012', value: 5 },
//          { year: '2013', value: 2 },
//          { year: '2014', value: 20 }
//        ],
//            xkey: 'year',
//            ykeys: ['value'],
//            labels: ['Value']
//        });

//        Morris.Donut({
//            element: 'donut-example',
//            data: [
//         { label: "Android", value: 12 },
//         { label: "iPhone", value: 30 },
//         { label: "Other", value: 20 }
//        ]
//        });

//        Morris.Bar({
//            element: 'bar-example',
//            data: [
//            { y: 'Jan 2014', a: 100, b: 90 },
//            { y: 'Feb 2014', a: 75, b: 65 },
//            { y: 'Mar 2014', a: 50, b: 40 },
//            { y: 'Apr 2014', a: 75, b: 65 },
//            { y: 'May 2014', a: 50, b: 40 },
//            { y: 'Jun 2014', a: 75, b: 65 }
//         ],
//            xkey: 'y',
//            ykeys: ['a', 'b'],
//            labels: ['Visitors', 'Conversions']
//        });

//    });
//});