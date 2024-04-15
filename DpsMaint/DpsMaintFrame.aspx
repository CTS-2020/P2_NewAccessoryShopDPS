<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DpsMaintFrame.aspx.cs" Inherits="DpsMaintFrame" %>

<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="application/xhtml+xml;charset=UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge"/>
    <title>Perodua DPS Application</title>
<script type="text/javascript" language="JavaScript" src="../jslib/coolmenus3.js"></script>
<script type="text/javascript" language="Javascript">

var AryWinOpened = new Array(); // Create array for child window opened
var AryWinOpenedLastIdx = 0; // This flag to keep track total number of child window opened
var lsignoutflag = 0;  // This flag to restrict the sign out process occurs once only, no double entry of audit log recorded
var lsignoutway = ""; // This flag to keep for what way the user was signed off
var clmlocation = "http://development.utem.edu.my/portal/clm_sistem.asp?msg=8";
function frameclose(x)
{
	for (var i=0; i<AryWinOpenedLastIdx; i++)
	{
		if (!AryWinOpened[i].closed) {
			AryWinOpened[i].close(); // Close all child window
		}
	}
	
	if (x == 1)  // This is true on every occurrence of window.close process, includes direct click on button "[ X ]" (close button).
	{
		if (lsignoutway=="")  // imply that no other way preset, the user straight away click on [X] button 
			lsignoutway = "Close Window";
		
			window.top.close(); // This statement will activate onunload event for the frameset tag
			window.opener.location = clmlocation;
					
	}else 
	{
		if (x == 0){ // This to consider user click "sign out" from the menu list.
			if (lsignoutflag == 0)  
			{
				ans=confirm("Do You Really Want To Sign Out From SMPU?");
				if (ans)
				{
					lsignoutflag = 1;
					lsignoutway = "Biasa"
					window.top.close(); // This statement will activate onunload event for the frameset tag
					window.opener.location = clmlocation;
				}
			}
		}else
			if (x == 2) { // This to consider user click on the "Sign Out" button at the timeout page.
					lsignoutway = "Session Timeout";
					window.top.close();  // This statement will activate onunload event for the frameset tag
			}
			
			if (x == 3) { // This to consider user click on the "Sign Out" button at the timeout page.
					lsignoutway = "User Kill Out"
					window.top.close();  // This statement will activate onunload event for the frameset tag
			}
	}
}

</script>
</head>
    <frameset cols="18%,*" frameborder="0">
        <frame src="DpsMaintMenu.aspx">
        <frameset rows="100%,*">
            <frame src="../BlankFrame.aspx" frameborder="0"  scrolling="auto" border="0" name="BottomRowFrame" id="BottomRowFrame" >
        </frameset>
    </frameset>
</html>