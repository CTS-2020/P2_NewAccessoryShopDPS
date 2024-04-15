//function ShowProgress(strFunction) {
//    setTimeout(function() {
//        //var modal = $('<div />');
//        //modal.addClass("modal" + strFunction);

//        var modal = document.getElementById('load' + strFunction);
//        //modal.class.add();
//        //modal.className = "modal" + strFunction;
//        modal.classList.add("modal" + strFunction);
//        //var body = document.getElementById('body' + strFunction);
//        //body.append(modal);
//        document.body.appendChild(modal);
//        var submit = document.getElementById(".loading" + strFunction);

//        //$('body').append(modal);
//        //var submit = $(".loading" + strFunction);
//        submit.show();
//        var top = Math.max($(frames).height() / 2 - submit[0].offsetHeight / 2, 0);
//        var left = Math.max($(frames).width() / 2 - submit[0].offsetWidth / 2, 0);
//        submit.css({ top: top, left: left });
//    }, 200);
//};
//$('form').live("submit", function() {
//    ShowProgress();
//});

//function Confirm(strConfirm)
//{
//    var confirm_value = document.createElement("INPUT");
//    confirm_value.type = "hidden";
//    confirm_value.name = "confirm_value";
//    if (confirm(strConfirm)) {
//        confirm_value.value = "Yes";
//    } else {
//        confirm_value.value = "No";
//    }
//    document.forms[0].appendChild(confirm_value);
//    ShowProgress("import");
//    //displayLoadingImage();
//    //disableButtonOnClick(this, 'buttonDisabled');
//};

//function ShowPopup()
//{
//    $('#Popup').show("slow");
//};

//ClientScriptManager script = Page.ClientScript;
//if (!script.IsStartupScriptRegistered(GetType(), "Show Popup"))
//{
//    script.RegisterStartupScript(GetType(), "Show Popup", "ShowPopup();", true);
//}

function ShowProgress(strFunction) {
    setTimeout(function() {
        var modal = $('<div />');
        modal.addClass("modal" + strFunction);
        $('body').append(modal);
        var submit = $(".loading" + strFunction);
        submit.show();
        var top = Math.max($(frames).height() / 2 - submit[0].offsetHeight / 2, 0);
        var left = Math.max($(frames).width() / 2 - submit[0].offsetWidth / 2, 0);
        submit.css({ top: top, left: left });
    }, 200);
};

function Confirm(strConfirm) {
    var confirm_value = document.createElement("INPUT");
    confirm_value.type = "hidden";
    confirm_value.name = "confirm_value";
    if (confirm(strConfirm)) {
        confirm_value.value = "Yes";
    } else {
        confirm_value.value = "No";
    }
    document.forms[0].appendChild(confirm_value);
};

function Print() {
    var value = getParameterByName("flag");
    if (value == "export") {
        window.print();
    }
};

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
};

//function displayLoadingImage() {
//    document.getElementById('imgLoading').style.display = "";
//    setTimeout('document.images["imgLoading"].src="../styles/images/icon/loading.gif"', 200);
//};

//function disableButtonOnClick(oButton, sCssClass) {
//    oButton.disabled = true;
//    oButton.setAttribute('className', sCssClass);
//    oButton.setAttribute('class', sCssClass);
//};