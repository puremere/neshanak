$("#languagetOption").change(function () {
    let id = $(this).val()
    if (id != '') {

        window.location.href = "/Home/ChangeLanguage?lang=" + id + "&actionstring=createItem";

    }
});
$("#addnew").click(function () {
    window.location.href = "/Home/createItem"
})
$(".default").click(function () {
    $('#imageFileHolder').trigger('click');
})


//
$("#imageFileHolder").on('change', function () {

    var input = this;
    var url = $(this).val()
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    var filename = url.substring(url.lastIndexOf('\\') + 1).toLowerCase();
    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
        var processID = makeid();
        var htmldiv = `<div  class="imageCover"> <img id="` + processID + `" src="#" /> </div>`;

        $("#imageHolder").append(htmldiv);
        var mydiv = document.getElementById(processID);

        let key = $("#fileupload").val();

        var reader = new FileReader();
        reader.addEventListener("load", function () {
            mydiv.src = reader.result;

        }, false);

        reader.readAsDataURL(input.files[0]);


        sendFile(key + ";;;" + processID, input.files[0]);

    }


});
var makeid = function () {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < 10; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
};
var sendFile = function (key, value) {

    let progressID = key.split(';;;')[1];
    key = key.split(';;;')[0];

    var ext = key.substring(key.lastIndexOf('.') + 1).toLowerCase();
    var filename = key.substring(key.lastIndexOf('\\') + 1).toLowerCase();
    let messageType = null;
    if (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg") {
        messageType = 'image';
    }
    else {
        messageType = 'file';
    }


    var formData = new FormData();
    var fileName = key
    formData.append('blob', value);
    formData.append('filename', fileName);
    let request = new XMLHttpRequest();
    request.open('POST', '/home/sendToServer');
    request.upload.addEventListener('progress', function (e) {

        let percent_complete = parseInt((e.loaded / e.total) * 100);

        let classname = '';
        if (percent_complete >= 50) {
            classname = 'over50';
        }




        let srt = `<div  class=" progress-circlle ` + classname + `  p` + percent_complete + ` "><span>` + percent_complete + `%</span><div class="left-half-clipper"><div class="first50-bar"></div><div class="value-bar"></div></div></div>`;
        var spg = $("#" + progressID).parent();

        spg.children('.progress-circlle').each(function () {
            //this.remove();
        })
        spg.append(srt);

        //console.log(e.loaded / e.total + "-" + percent_complete + "%");
    });

    // AJAX request finished event
    request.addEventListener('load', function (e) {

        var ext = key.substring(key.lastIndexOf('.') + 1).toLowerCase();
        var filename = key.substring(key.lastIndexOf('\\') + 1).toLowerCase();


        var ul = $(".messages ul");

        let rsp = request.response;

        var spg = $("#" + progressID).parent();
        var name = "" + rsp;


        spg.children('.progress-circlle').each(function () {
            this.remove();
        })
        let check = `<img  onclick = removeImage("` + progressID + `***` + name + `")   id="R` + progressID + `" class="removeImage" src="/images/trash.png" style="position:absolute; bottom:0;top:0;left:0;right:0;margin:auto" /> <i class="fa fa-remove" style="font-size: 12px;position: absolute;bottom: 5px;left: 0px;color: white;"></i>`;
        spg.append(check);
        //onclick=removeImage(` + rsp + `)
    })
    // send POST request to server side script
    request.send(formData);

};
$(".removeImage").click(function () {
    var element = $(this)
    var name = element.attr('id');
    $.ajax({
        url: "/home/removeimage",
        data: {
            id: name
        },
        success: function () {
            element.parent().remove();
        }
    })
    //            $.ajax({
    //                url: "/home/removeimage",
    //                data: {
    //                    id: name
    //                },
    //                success: function () {
    //                    alert("#P" + name);
    //                    //$("#P" + name).remove();
    //.                }
    //            })
});
var removeImage = function (DAT) {
    let id = "R" + DAT.split('***')[0];
    let name = DAT.split('***')[1];
    $.ajax({
        url: "/home/removeimage",
        data: {
            id: name
        },
        success: function () {
            $("#" + id).parent().remove();
        }
    })
}
//

$(document).ready(function () {
    $(".fa-file").addClass("act")
})
getLocation();
function showLocation(position) {
    var lat = position.coords.latitude;
    var lon = position.coords.longitude;
    var mymap = L.map('mapid', { attributionControl: false }).setView([51.505, -0.09], 13);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoibWVyaW9saSIsImEiOiJja2p6dXhmdngwYmVvMnZwY2lyc2htNGxhIn0.Y9t__Bll6Fw9b9amb_rcHg',

    }).addTo(mymap);
    mymap.panTo(new L.LatLng(lat, lon));


    var theMarker = {};
    mymap.on('click', function (e) {

        lat = e.latlng.lat;
        lon = e.latlng.lng;

        console.log("You clicked the map at LAT: " + lat + " and LONG: " + lon);
        //Clear existing marker,

        if (theMarker != undefined) {
            mymap.removeLayer(theMarker);
        };

        //Add a marker to show where you clicked.
        theMarker = L.marker([lat, lon]).addTo(mymap);
        $("#lat").val(lat);
        $("#lon").val(lon);
    });
}
function errorHandler(err) {


    var mymap = L.map('mapid', { attributionControl: false }).setView([51.505, -0.09], 13);
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token={accessToken}', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1IjoibWVyaW9saSIsImEiOiJja2p6dXhmdngwYmVvMnZwY2lyc2htNGxhIn0.Y9t__Bll6Fw9b9amb_rcHg',

    }).addTo(mymap);
    mymap.panTo(new L.LatLng('35.7442005749', '51.473131743990'));


    var theMarker = {};
    mymap.on('click', function (e) {

        lat = e.latlng.lat;
        lon = e.latlng.lng;

        console.log("You clicked the map at LAT: " + lat + " and LONG: " + lon);
        //Clear existing marker,

        if (theMarker != undefined) {
            mymap.removeLayer(theMarker);
        };

        //Add a marker to show where you clicked.
        theMarker = L.marker([lat, lon]).addTo(mymap);
        $("#lat").val(lat);
        $("#lng").val(lon);
    });



    if (err.code == 1) {
        //alert("Error: Access is denied!");
    } else if (err.code == 2) {
        //alert("Error: Position is unavailable!");
    }





}
function getLocation() {
    if (navigator.geolocation) {
        // timeout at 60000 milliseconds (60 seconds)
        var options = { timeout: 60000 };
        navigator.geolocation.getCurrentPosition
            (showLocation, errorHandler, options);


    } else {
        alert("Sorry, browser does not support geolocation!");
    }
}
$("#mapid").height("250px");

