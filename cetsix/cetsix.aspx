<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cetsix.aspx.cs" Inherits="cetsix_cetsix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>英语六级词汇查询</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/10086bank.css" type="text/css" rel="stylesheet">
    <link rel="stylesheet" href="css/iosOverlay.css">
</head>
<body>
    <div align="center">
        <h3 style="color: Red">
            感谢来到这里的--你/您，有好的建议请联系我,QQ:502048227</h3>
        <a style="color:Red;font-size: 35px;">英文/中文：</a>
        <input type="text" style="font-size: 35px; height: 50px; width:360px" value="ing"  id="txtwords" />
        &nbsp;<img alt="search" id="btns" src="img/btn.png" style="height:60px; width:60px;" />
        <a id="laf" style="color:Red; font-size:15px"></a>
    </div>
    <div id="divresult">
    </div>
    <script type="text/javascript" src="../blog/JS/jquery-1.8.0.js"></script>
    <script type="text/javascript" src="js/modernizr-2.0.6.min.js"></script>
    <script type="text/javascript" src="js/iosOverlay.js"></script>
    <script type="text/javascript" src="js/spin.min.js"></script>
    <script type="text/javascript" src="js/prettify.js"></script>
    <script type="text/javascript" src="js/custom.js"></script>

    <script type="text/javascript" src="js/jquery-1.8.0.min.js"></script>
    <script type="text/javascript">
        var searchwors = "a";
        $(function () {

            $("#btns").click(function () {
                if ($("#txtwords").val() == "") {
                    $(".ui-ios-overlay").hide();
                    $("#txtwords").focus();
                    $("#laf").html("请输入");
                    return false;
                }
                else if (searchwors == $("#txtwords").val()) {
                    $(".ui-ios-overlay").hide();
                    $("#txtwords").focus();
                    $("#laf").html("请更换");
                    return false;
                }
                GetS();
            });

            $("#txtwords").keyup(function (e) {
                if (e.keyCode == 13) {
                    if ($("#txtwords").val() == "") {
                        $(".ui-ios-overlay").hide();
                        $("#txtwords").focus();
                        $("#laf").html("请输入");
                        return false;
                    }
                    else if (searchwors == $("#txtwords").val()) {
                        $(".ui-ios-overlay").hide();
                        $("#txtwords").focus();
                        $("#laf").html("请更换");
                        return false;
                    }
                    GetS();
                }
            });

            function GetS() {
                $(".ui-ios-overlay").show();
                searchwors = $("#txtwords").val();
                $("#divresult").html("");
                $.post("Ajax.aspx", { "type": "get", "words": $("#txtwords").val() }, function (redata) {

                    if (redata == null || redata == "") {
                        $(".ui-ios-overlay").hide();
                        $("#laf").html("Counts:0");
                        return;
                    }

                    var obj = $.parseJSON(redata);
                    if (obj == null || obj == "") {
                        $(".ui-ios-overlay").hide();
                        $("#laf").html("Counts:0");
                        return;
                    }

                    var sampelist = [];
                    var result = "";
                    $("#laf").html("Counts:" + obj.length);
                    for (var i = 0; i < obj.length; i++) {

                        if (decodeURIComponent(obj[i].meaning).length < 100)
                            result = '<ul id="tabs"><li><a style="font-size:20px;color:red">' + decodeURIComponent(obj[i].words) + '</a></li></ul>' + '<div id="content"><div id="tab1"><h2>' + decodeURIComponent(obj[i].meaning) + '</h2>';
                        else
                            result = '<ul id="tabs"><li><a style="font-size:20px;color:red">' + decodeURIComponent(obj[i].words) + '</a></li></ul>' + '<div id="content"  style="height:500px" ><div id="tab1"><h2>' + decodeURIComponent(obj[i].meaning) + '</h2>';

                        sampelist = decodeURIComponent(obj[i].lx).split("。");
                        if (sampelist != null && sampelist.length > 1) {
                            result = result + '<br/><h3>' + sampelist[0] + "<br/>" + sampelist[1] + '</h3>';
                        }
                        else {
                            result = result + '<br/><h3>' + decodeURIComponent(obj[i].lx) + '</h3>';
                        }

                        result = result + ('</div></div>');
                        $("#divresult").append("<br/>" + result);
                        if (obj.length == (i + 1)) {
                            $("#divresult").append("<br/>");
                        }
                        result = null;
                        sampelist = null;
                        sampelist = [];
                    }
                    redata = null;
                    obj = null;
                    $(".ui-ios-overlay").hide();
                })
            }

        })
    </script>
</body>
</html>
