isExpanded = false;

function getIndex(el) {
	ind = null;
	for (i=0; i<document.layers.length; i++) {
		whichEl = document.layers[i];
		if (whichEl.id == el) {
			ind = i;
			break;
		}
	}
	return ind;
}

function arrange() {
	nextY = document.layers[firstInd].pageY + document.layers[firstInd].document.height;
	for (i=firstInd+1; i<document.layers.length; i++) {
		whichEl = document.layers[i];
		if (whichEl.visibility != "hide") {
			whichEl.pageY = nextY;
			nextY += whichEl.document.height;
		}
	}
}

function initIt(){
	if (NS4) {
		for (i=0; i<document.layers.length; i++) {
			whichEl = document.layers[i];
			if (whichEl.id.indexOf("Child") != -1)whichEl.visibility = "hide";
		}
		arrange();
	} else if(IE4) {
		tempColl = document.all.tags("DIV");
		for (i=0; i<tempColl.length; i++) {
			if (tempColl(i).className == "child")tempColl(i).style.display = "none";
		}
	}
}

function expandIt(el) {
	whichEl = el + "Child";
	if (!can_display) return;
	if (IE4 || NS6)
	{
	  expandIE(whichEl)
	}
	else
	{
	  expandNS(whichEl)
	}
}

function expandIE(whichEl) {
	if(document.getElementById){
	var el = document.getElementById(whichEl);
	var ar = document.getElementById("masterdiv").getElementsByTagName("div"); //DynamicDrive.com change
		if(el.style.display != "block"){ //DynamicDrive.com change
			for (var i=0; i<ar.length; i++){
				if (ar[i].className=="child") //DynamicDrive.com change
				{}//ar[i].style.display = "none";
			}
			el.style.display = "block";
		}else{
			//el.style.display = "none";
		}

// for empty Child - no sub like prepaid, stock

		if (el.id == "el0Child")
		{
			//el.style.display = "none";
//			alert(el.id+":"+el.style.display)
		}
	}
	
}

function expandNS(whichEl) {
/*
	if(whichEl.visibility != "hide"){
	alert("tTEST")
		whichEl.visibility = "hide";
	var el = document.getElementById(whichEl);
	var ar = document.getElementById("masterdiv").getElementsByTagName("span"); //DynamicDrive.com change
		if(el.visibility != "show"){ //DynamicDrive.com change
			for (var i=0; i<ar.length; i++){
				if (ar[i].className=="child") //DynamicDrive.com change
				ar[i].visibility = "hide";
			}
			el.visibility = "show";
		}else{
			el.visibility = "hide";
		}
	}
*/
/*
	whichEl = eval("document." + whichEl);
	if (whichEl.style.visibility == "hide") {
		whichEl.style.visibility = "show";
	} else {
		whichEl.style.visibility = "hide";
	}
	*/
	arrange();
}

function showAll() {
	for (i=firstInd; i<document.layers.length; i++) {
		whichEl = document.layers[i];

		whichEl.visibility = "show";
	}
}

function expandAll(isBot) {
	newSrc = (isExpanded) ? "" : "";//"/maybank_gif/navbar/plus.jpg" : "/maybank_gif/navbar/minus.jpg";

	if (NS4) {
        document.images["imEx"].src = newSrc;
		for (i=firstInd; i<document.layers.length; i++) {
			whichEl = document.layers[i];
			if (whichEl.id.indexOf("Parent") != -1) {
				whichEl.document.images["imEx"].src = newSrc;
			}
			if (whichEl.id.indexOf("Child") != -1) {
				//whichEl.visibility = (isExpanded) ? "hide" : "show";
				}
		}

		arrange();
		if (isBot && isExpanded) scrollTo(0,document.layers[firstInd].pageY);
	} else {
		if(IE4) {
			divColl = document.all.tags("DIV");
		} else {
			divColl = document.getElementsByTagName("DIV");
		}
/*
		for (i=0; i<divColl.length; i++) {
			if (divColl(i).className == "child") {
				divColl(i).style.display = (isExpanded) ? "none" : "block";
			}
		}
		*/
		imColl = document.images.item("imEx");
		for (i=0; i<imColl.length; i++) {
			imColl(i).src = newSrc;
		}
	}

	isExpanded = !isExpanded;
}

// NEW FUNCTION FOR NS6
function getChildLayer(el) {
	if(document.getElementById)
		if(document.getElementById(el+"Child"))
			return document.getElementById(el+"Child");

	if(document.all)
		if(document.all[el+"Child"])
			return document.all[el+"Child"];

	return new Object();
}


with (document) {
	write("<STYLE TYPE='text/css'>");

	{
		
		write("A:hover {color:#CCCCCC;}"); //white
	}
	write("</STYLE>");
}

onload = initIt;


