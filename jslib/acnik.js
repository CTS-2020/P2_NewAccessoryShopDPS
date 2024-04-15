/*******************************************************************************/
/*	credits:	                                                               */
/*			zichun (http://www.codetools.com/jscript/jsactb.asp?print=true)    */
/*			Chris Justus                                                       */
/*                                                                             */
/*	implementation:                                                            */
/*			nikola.i@siol.net                                                  */
/*******************************************************************************/

var ACNIK_EVT_ON_BLUR		= 10000;
var ACNIK_EVT_ON_KEY_DOWN	= 10001;
var ACNIK_EVT_ON_KEY_UP		= 10002;
var ACNIK_EVT_ON_MOUSE_OVER	= 10003;
var ACNIK_EVT_ON_SET_VALUE	= 10004;


var IDX_FIELD	= 0;
var IDX_RESULT	= 1;
var IDX_URL		= 2;
var IDX_VALUES	= 3;
var IDX_TITLES	= 4;
var IDX_RECT	= 5;
var IDX_NITEMS	= 6;
var IDX_OFFSET	= 7;
var IDX_MTITLES	= 8;
var IDX_MVALUES	= 9;
var IDX_MBEGIN	= 10;
var IDX_KEYSEL	= 11;
var IDX_FORCED	= 12;
var IDX_EVENT	= 13;
var IDX_CLEAR	= 14;


var acnikObjs			= new Array();
var acnikPrevSelection	= null;
var acnikOldKDHandler	= document.onkeydown;

//this is the function that installs the dropdown functionality upon a field
//	text field of drop; 
//	result field where the value is stored, 
//	array of titles to show in drop, 
//	array of values that must be inserted to some other field if needed, 
//	url if remotly feched values and titles, 
//	num items on drop
function acnikRegister(formTextObj, formResultObj, textTitles, textValues, url) {
	
	if(!formTextObj) {
		alert('WARN: no form object passed in acnikRegister function');
		return;
	}
	
	if(!formResultObj) {
		alert('WARN: no object to store result passed in acnikRegister function');
		return;
	}
	
	if(!url && (!textTitles || !textValues)) {
		alert('WARN: no titles and values or url to get them passed in acnikRegister function');
		return;
	}
	
	//find first empty slot
	var iindex = acnikObjs.length;
	for(var i = 0; i < acnikObjs.length; i++)
		if(acnikObjs[i] == null) {
			iindex = i;
			break;
		}
		
	
	acnikObjs[iindex]	= new Array();
	acnikObjs[iindex][IDX_FIELD] = formTextObj;
	acnikObjs[iindex][IDX_RESULT] = formResultObj;
	if(url) {
		acnikObjs[iindex][IDX_URL] 		= url;
		acnikObjs[iindex][IDX_TITLES]	= null;
		acnikObjs[iindex][IDX_VALUES]	= null;
	} else {
		acnikObjs[iindex][IDX_URL]		= null;
		acnikObjs[iindex][IDX_TITLES]	= textTitles;
		acnikObjs[iindex][IDX_VALUES]	= textValues;
	}
	
	acnikObjs[iindex][IDX_RECT] = acnikObjRect(acnikObjs[iindex][IDX_FIELD]);
	
	acnikObjs[iindex][IDX_NITEMS]	= 5;
	acnikObjs[iindex][IDX_OFFSET]	= -1;
	acnikObjs[iindex][IDX_MTITLES]	= null;
	acnikObjs[iindex][IDX_MVALUES]	= null;
	acnikObjs[iindex][IDX_MBEGIN]	= true;
	acnikObjs[iindex][IDX_FORCED]	= false;
	acnikObjs[iindex][IDX_KEYSEL]	= false;
	acnikObjs[iindex][IDX_CLEAR]	= false;
	acnikObjs[iindex][IDX_EVENT] 	= new Array();
	
	document.onkeydown	= acnikOnKeyDown;
	document.onkeyup	= acnikOnKeyUp;
	acnikObjs[iindex][IDX_FIELD].onblur = acnikOnBlur;
	
}

function acnikMatchBegining(bool) {
	if(bool)
		acnikObjs[acnikObjs.length - 1][IDX_MBEGIN] = true;
	else
		acnikObjs[acnikObjs.length - 1][IDX_MBEGIN] = false;
}


function acnikForceInput(bool) {
	if(bool)
		acnikObjs[acnikObjs.length - 1][IDX_FORCED] = true;
	else
		acnikObjs[acnikObjs.length - 1][IDX_FORCED] = false;
}


function acnikClearOnSetValue(bool) {
	if(bool)
		acnikObjs[acnikObjs.length - 1][IDX_CLEAR] = true;
	else
		acnikObjs[acnikObjs.length - 1][IDX_CLEAR] = false;
}


function acnikSetNumItems(num) {
	
	if(!isNaN(num) && num > 0)
		acnikObjs[acnikObjs.length - 1][IDX_NITEMS] = num;
	
}

function acnikRegisterEventFunction(eventName, functionName) {
	
	if(!eventName || !functionName) return;
	var idx		= acnikObjs.length - 1;
	var eidx	= acnikObjs[idx][IDX_EVENT].length
	
	if(!acnikObjs[idx]) return;
	
	acnikObjs[idx][IDX_EVENT][eidx] = new Array(eventName, functionName);
}


function acnikObjRect(obj) {
	var objtop		= 0;
	var objleft		= 0;
	var objwidth	= 0;
	var objheight	= 0;
	
	if(obj) {
		objwidth	=	obj.offsetWidth;
		objheight	=	obj.offsetHeight;
	}
	
	while(obj) {
		objtop		+=	obj.offsetTop;
		objleft		+=	obj.offsetLeft;
		
		obj = obj.offsetParent;
	}
	
	return new Array(objtop, objleft, objwidth, objheight);
}


function acnikObjHideDrops() {
	
	for(var i = 0; i < acnikObjs.length; i++) {
		while(document.getElementById('acnik_table' + i)) {
			document.body.removeChild(document.getElementById('acnik_table' + i)); 
		}
	}
}

function acnikObjShowHidden() {
	
	for(var i = 0; i < document.forms.length; i++) {
		var objForm = document.forms[i];
		if(objForm && objForm.elements)
			for(var j = 0; j < objForm.elements.length; j++)
				if(objForm.elements[j] && objForm.elements[j].style)
					if(objForm.elements[j].style.visibility == 'hidden') { 
						objForm.elements[j].style.visibility = 'visible';
					}
	}
}


function acnikGetCaretEnd(obj){
	if(typeof obj.selectionEnd != 'undefined'){
		return obj.selectionEnd;
	}else if(document.selection&&document.selection.createRange){
		var M=document.selection.createRange();
		var Lp=obj.createTextRange();
		Lp.setEndPoint('EndToEnd',M);
		var rb=Lp.text.length;
		if(rb>obj.value.length){
			return -1;
		}
		return rb;
	}
}


function acnikGetCaretStart(obj){
	if(typeof obj.selectionStart != 'undefined'){
		return obj.selectionStart;
	}else if(document.selection&&document.selection.createRange){
		var M=document.selection.createRange();
		var Lp=obj.createTextRange();
		Lp.setEndPoint('EndToStart',M);
		var rb=Lp.text.length;
		if(rb>obj.value.length){
			return -1;
		}
		return rb;
	}
}


function acnikSetCaret(obj,l){
	obj.focus();
	if (obj.setSelectionRange){
		obj.setSelectionRange(l,l);
	}else if(obj.createTextRange){
		m = obj.createTextRange();		
		m.moveStart('character',l);
		m.collapse();
		m.select();
	}
}


String.prototype.addslashes = function(){
	return this.replace(/(["\\\.\|\[\]\^\*\+\?\$\(\)])/g, '\\$1');
}


String.prototype.trim = function () {
    return this.replace(/^\s*(\S*(\s+\S+)*)\s*$/, '$1');
};


function acnikHideFormElts(sourceObj, objForm) {
	
	var rect2 = acnikObjRect(sourceObj);
	
	for(var i = 0; i < objForm.elements.length; i++) {
		if(objForm.elements[i].type.indexOf('select') != -1) {
			var rect1 = acnikObjRect(objForm.elements[i]);
			
			if(		(rect2[0] + rect2[3] >= rect1[0] && rect2[1] + rect2[2] >= rect1[1])
				&&	(rect2[0] <= rect1[0] && rect2[1] <= rect1[1])){
				objForm.elements[i].style.visibility = 'hidden';
			}
		}
	}
}


function acnikObjHttpQString(obj) {
	
	var qstr	= '';
	if(!obj.form) return qstr;
	
	var objForm = obj.form;
	var added1 = false;
	for(var i = 0; i < objForm.elements.length; i++) {
		
		if(objForm.elements[i].type.indexOf('select') != -1) {
			
			if(added1 && objForm.elements[i].options.selectedIndex >= 0) 
				qstr += '&';
			
			if(objForm.elements[i].options) {
				if(objForm.elements[i].multiple) {
					
					var added2 = false;
					for(var j = 0; j < objForm.elements[i].options.length; j++) {
						if(		objForm.elements[i].options[j].value 
							&&	objForm.elements[i].options[j].selected) {
							
							if(!added2)
								qstr += objForm.elements[i].name + '[]=';
							else
								qstr += '&' + objForm.elements[i].name + '[]=';
							
							var val = objForm.elements[i].options[j].value;
							
							if(escape) qstr += escape(val)
							else if(encodeURIComponent) qstr += encodeURIComponent(val);
							
							added2 = true;
						}
					}
					
				} else if (objForm.elements[i].options.selectedIndex >= 0) {
					
					var oindex = objForm.elements[i].options.selectedIndex;
					var val = objForm.elements[i].options[oindex].value;
					if(val) {
						qstr += objForm.elements[i].name + '=';
						if(escape) qstr += escape(val)
						else if(encodeURIComponent) qstr += encodeURIComponent(val);
					}
					
					added1 = true;
				}
			}
			
		} else if(objForm.elements[i].value) {
			if(added1) qstr += '&';
		
			qstr += objForm.elements[i].name + '=';
			
			if(escape) qstr += escape(objForm.elements[i].value)
			else if(encodeURIComponent) qstr += encodeURIComponent(objForm.elements[i].value);
			
			added1 = true;
		}
		
	}
	
	return qstr;
}


function acnikObjHttpRequest(obj, uri) {
	
	var xmlHttp = null;
	try{
		xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
	}catch(e){
		try{
			xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
		} catch(oc){
			xmlHttp = null
		}
	}
	
	if(!xmlHttp && typeof XMLHttpRequest != 'undefined') {
		xmlHttp = new XMLHttpRequest()
	}
	
	if(xmlHttp && xmlHttp.readyState != 0){
		xmlHttp.abort()
	}
	
	if(xmlHttp){
		var qstr = acnikObjHttpQString(obj);
		if(qstr.length > 0) uri += '?' + qstr;
		//xmlHttp.setRequestHeader('Cookie', document.cookie);
		
		xmlHttp.open('GET', uri, true);
		xmlHttp.onreadystatechange = function() {
			if(xmlHttp.readyState == 4 && xmlHttp.responseText) {
				eval(xmlHttp.responseText);
			}
		}
		xmlHttp.send(null);
	}
}


function acnikObjHttpCallback(objIndexOrName, arrayTitles, arrayValues) {
	
	if(!objIndexOrName) return;
	
	if(isNaN(objIndexOrName)) {
		
		for(var i = 0; i < acnikObjs.length; i++) {
			
			if(		acnikObjs[i][IDX_FIELD] 
				&&	acnikObjs[i][IDX_FIELD].name == objIndexOrName) {
				
				var obj = acnikObjs[i][IDX_FIELD];
				if(!obj) return;
				objIndexOrName = i;
				break;
			}
		}
	
	} else {
	
		if(objIndex < 0) return;
		var obj = acnikObjs[objIndexOrName][IDX_FIELD];
		if(!obj) return;
		
	}
	
	acnikResetMachedValues(objIndexOrName);
	acnikObjs[objIndexOrName][IDX_TITLES] = arrayTitles;
	acnikObjs[objIndexOrName][IDX_VALUES] = arrayValues;
	acnikGetNearest(objIndexOrName);
	acnikObjShowDrop(objIndexOrName, true, false);
}


function acnikGetNearest(objIndex) {
	
	var obj		= acnikObjs[objIndex][IDX_FIELD];
	var titles	= acnikObjs[objIndex][IDX_TITLES];
	var values	= acnikObjs[objIndex][IDX_VALUES];
	
	if(!obj.value) return 0;
	if(obj.value.length <= 0) return 0;
	if(!titles) return 0;
	if(titles.length <= 0) return 0;
	
	var srch = obj.value.trim().addslashes();
	var rx = acnikObjs[objIndex][IDX_MBEGIN]?new RegExp('^'+srch, 'i'):new RegExp(srch, 'i');
	
	titles1 = new Array();
	values1 = new Array();
		
	for(var i = 0; i < titles.length; i++) 
		if(titles[i] && titles[i].trim().search(rx) >= 0) {
			var j = titles1.length;
			titles1[j] = titles[i];
			values1[j] = values[i];
		}
		
	acnikObjs[objIndex][IDX_MTITLES] = titles1;
	acnikObjs[objIndex][IDX_MVALUES] = values1;
	
	return titles1.length;
}


function acnikResetMachedValues(objIndex) {
	
	if(acnikObjs[objIndex][IDX_MTITLES]) 
		acnikObjs[objIndex][IDX_MTITLES] = null;
	
	if(acnikObjs[objIndex][IDX_MVALUES])
		acnikObjs[objIndex][IDX_MVALUES] = null;
		
	acnikObjs[objIndex][IDX_OFFSET] = -1;
	
}

function acnikGetStrStart(srch, subject) {
	
	if(!srch) return -1;
	if(srch.length <= 0) return -1;
	if(!subject) return -1;
	if(subject.length <= 0) return -1;
	
	srch.trim().addslashes();
	var rx = new RegExp(srch, 'i');
	
	return subject.trim().search(rx);
}


function acnikObjSetOffset(objIndex, offset) {
	
	var obj = acnikObjs[objIndex][IDX_FIELD];
	if(!obj) return;
	
	if(!acnikObjs[objIndex][IDX_MTITLES]) {
		acnikObjs[objIndex][IDX_OFFSET] = -1;
		return;
	}
	
	if(!acnikObjs[objIndex][IDX_MTITLES].length) {
		acnikObjs[objIndex][IDX_OFFSET] = -1;
		return;
	}
	
	acnikObjs[objIndex][IDX_OFFSET] = offset;
}


function acnikObjGetOffset(objIndex, offset) {
	
	var obj = acnikObjs[objIndex][IDX_FIELD];
	if(!obj) return -1;
	
	if(!acnikObjs[objIndex][IDX_MTITLES]) return  -1;
	if(!acnikObjs[objIndex][IDX_MTITLES].length) return -1;
	
	return acnikObjs[objIndex][IDX_OFFSET];
}


function acnikObjIncOffset(objIndex) {
	
	var obj = acnikObjs[objIndex][IDX_FIELD];
	if(!obj) return;
	
	if(!acnikObjs[objIndex][IDX_MTITLES]) return;
	if(!acnikObjs[objIndex][IDX_MTITLES].length) return;
	
	var titles = acnikObjs[objIndex][IDX_MTITLES];
	
	if(titles.length - 1 > acnikObjs[objIndex][IDX_OFFSET]) 
		acnikObjs[objIndex][IDX_OFFSET]++;
}


function acnikObjDecOffset(objIndex) {
	
	var obj = acnikObjs[objIndex][IDX_FIELD];
	if(!obj) return;
	
	if(!acnikObjs[objIndex][IDX_MTITLES]) return;
	if(!acnikObjs[objIndex][IDX_MTITLES].length) return;
	
	var titles = acnikObjs[objIndex][IDX_MTITLES];
	
	if(acnikObjs[objIndex][IDX_OFFSET] > 0) 
		acnikObjs[objIndex][IDX_OFFSET]--;
}


function acnikObjSetValue(objIndex) {

	var obj		= acnikObjs[objIndex][IDX_FIELD];
	var resobj	= acnikObjs[objIndex][IDX_RESULT];
	if(!obj) return;
	
	var otitle	= null;
	var offset	= acnikObjs[objIndex][IDX_OFFSET]; 
	var force	= acnikObjs[objIndex][IDX_FORCED];
	
	var titles = acnikObjs[objIndex][IDX_MTITLES];
	if(!titles && !force) {
		return;
	} else if(!titles && force) {
		titles = acnikObjs[objIndex][IDX_TITLES];
		acnikObjs[objIndex][IDX_OFFSET] = 0;
		offset = 0;
	} else if(!titles.length && force) {
		titles = acnikObjs[objIndex][IDX_TITLES];
		acnikObjs[objIndex][IDX_OFFSET] = 0;
		offset = 0;
	}
	
	var values = acnikObjs[objIndex][IDX_MVALUES];
	if(!values && !force) {
		return;
	}else if(!values && force) {
		values = acnikObjs[objIndex][IDX_VALUES];
		acnikObjs[objIndex][IDX_OFFSET] = 0;
		offset = 0;
	} else if(!values.length && force) {
		values = acnikObjs[objIndex][IDX_VALUES];
		acnikObjs[objIndex][IDX_OFFSET] = 0;
		offset = 0;
	}
	
	if(titles && titles.length) {
		if((offset < 0 || offset >= titles.length) && force) {
			acnikObjs[objIndex][IDX_OFFSET] = 0;
			offset = 0;
		}
		
		if(offset > -1 && offset < titles.length && titles[offset]) {
			otitle = titles[offset];
			obj.value = otitle;
		} else {
			if(force) obj.value = '';
		}
		
		if(acnikObjs[objIndex][IDX_CLEAR]) obj.value = '';
	}
		
	if(!resobj) return;
	
	if(values && values.length) {
		if((offset < 0 || offset >= values.length) && force) {
			acnikObjs[objIndex][IDX_OFFSET] = 0;
			offset = 0;
		}
			
		var rvalue = null;
		if(offset > -1 && offset < values.length && values[offset])
			rvalue = values[offset];
		else {
			rvalue = '';
			if(!force) return;
		}
			
		if(resobj.type.indexOf('select') != -1 && otitle) {
			var found = false;
			for(var i = 0; i < resobj.options.length; i++) {
				if(resobj.options[i].value == rvalue) {
					found = true;
					break;
				}
			}
			
			if(!found) {
				resobj.options[resobj.options.length] = new Option(otitle, rvalue);
			}
		} else {
			resobj.value = rvalue;
		}
	}
	
	acnikAfterEvent(objIndex, ACNIK_EVT_ON_SET_VALUE);
}


function acnikObjShowHeader(enabled, table, objIndex) {
	
	var colspan = 3;
	
	if(enabled) {
		var tr			= table.insertRow(-1);
		var td			= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_ls_prev';
		td.className	= 'acnikDropEltPrevLeftS';
		td.innerHTML	= '';
		td.onmousedown	= acnikOnMouseDown;
		td.setAttribute('IDX', objIndex);
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_ms_prev';
		td.className	= 'acnikDropEltPrevMiddleS';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_rs_prev';
		td.className	= 'acnikDropEltPrevRightS';
		td.innerHTML	= '';
		
		
	} else {
		var tr			= table.insertRow(-1);
		var td			= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_l_prev';
		td.className	= 'acnikDropEltPrevLeft';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_m_prev';
		td.className	= 'acnikDropEltPrevMiddle';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_r_prev';
		td.className	= 'acnikDropEltPrevRight';
		td.innerHTML	= '';
	}
	
	return colspan;
}


function acnikObjShowFooter(enabled, table, objIndex) {
	
	if(enabled) {
		var tr			= table.insertRow(-1);
		var td			= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_ls_next';
		td.className	= 'acnikDropEltNextLeftS';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_ms_next';
		td.className	= 'acnikDropEltNextMiddleS';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_rs_next';
		td.className	= 'acnikDropEltNextRightS';
		td.innerHTML	= '';
		td.onmousedown	= acnikOnMouseDown;
		td.setAttribute('IDX', objIndex);
		
	} else {
		var tr			= table.insertRow(-1);
		var td			= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_l_next';
		td.className	= 'acnikDropEltNextLeft';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_m_next';
		td.className	= 'acnikDropEltNextMiddle';
		td.innerHTML	= '';
		
		td				= tr.insertCell(-1);
		td.id			= 'acnik_row' + objIndex + '_r_next';
		td.className	= 'acnikDropEltNextRight';
		td.innerHTML	= '';
	}
	
}


function acnikObjShowDrop(objIndex, callback, keysel) {
	if(objIndex < 0) return;
	var obj = acnikObjs[objIndex][IDX_FIELD];
	if(!obj) return;
	
	if(!obj.value) return;
	
	if(acnikObjs[objIndex][IDX_URL] && !callback) {
		acnikObjHttpRequest(obj, acnikObjs[objIndex][IDX_URL]);
	}
	
	var rect = acnikObjRect(obj);
	
	var tab 			= document.createElement('table');
	tab.cellSpacing		= '0px';
	tab.cellPadding		= '0px';
	tab.style.position	= 'absolute';
	tab.style.top 		= (rect[0] + rect[3] + 1) + 'px';
	tab.style.left 		= rect[1] + 'px';
	tab.style.width		= rect[2] + 'px';
	tab.id 				= 'acnik_table' + objIndex;
	
	document.body.appendChild(tab);
	
	var titles = acnikObjs[objIndex][IDX_MTITLES];
	var values = acnikObjs[objIndex][IDX_MVALUES];
		
	if(titles && titles.length && values && values.length) {
		
		var nitems = acnikObjs[objIndex][IDX_NITEMS];
		if(nitems > titles.length) {
			nitems = titles.length;
		}
		
		var start = acnikObjs[objIndex][IDX_OFFSET] - Math.round(nitems / 2);
		if(start < 0 || start > titles.length - 1) {
			start = 0;
		}
		
		if(start + nitems > titles.length) {
			start = titles.length - nitems;
		}
		
		var colspan = 1;
		if(start > 0)
			colspan = acnikObjShowHeader(true, tab, objIndex);
		else
			colspan = acnikObjShowHeader(false, tab, objIndex);
		
		var i = 0;
		for(i = start; i < start + nitems; i++) {
			var tr			= tab.insertRow(-1);
			var td			= tr.insertCell(-1);
	
			td.onmouseover	= acnikOnMouseOver; 
			td.onmousedown	= acnikOnMouseDown;
			td.setAttribute('IDX_OFFSET', i);
			td.setAttribute('IDX', objIndex);
			td.setAttribute('nowrap', 'true');
	
			td.id			= 'acnik_row_' + objIndex + '_' + i;
			td.colSpan		= colspan;
			

			if(i == acnikObjs[objIndex][IDX_OFFSET]) {
				td.className	= 'acnikDropEltSelected';
			} else {
				td.className	= 'acnikDropElt';
			}
			
			var ititle = titles[i];
			var idx = acnikGetStrStart(obj.value, ititle);
			if(idx >= 0) {
				var str = '';
				str	= ititle.substring(0, idx);
				str	+= '<span id=\"acnik_span_'+objIndex+'_'+i+'\" class=\"acnikMatched\">'; 
				str	+= ititle.substring(idx, idx + obj.value.length);
				str	+= '</span>';
				str	+= ititle.substring(idx + obj.value.length, ititle.length);
				td.innerHTML = str;
			} else {
				td.innerHTML	= ititle;
			}
		}
		
		if(i < titles.length) 
			acnikObjShowFooter(true, tab, objIndex);
		else
			acnikObjShowFooter(false, tab, objIndex);
		
		var tabRect			= acnikObjRect(tab);
		tab.style.width		= (tabRect[2] > rect[2])?tabRect[2] + 'px' : rect[2] + 'px';
		//alert(tab.style.width);
		
		acnikHideFormElts(tab, acnikObjs[objIndex][IDX_FIELD].form);
	}
	
}


function acnikOnBlur(evt) {
	
	if (!evt) evt = event;
	var kc = evt.keyCode;
	var targetObj = (evt.target) ? evt.target : evt.srcElement;
	
	var currIdx = -1;
	for(var i = 0; i < acnikObjs.length; i++) {
		if(acnikObjs[i][IDX_FIELD] == targetObj) {
			currIdx = i;
			break;
		}
	}
	
	acnikObjHideDrops();
	acnikObjShowHidden();
	acnikObjSetValue(currIdx);
	acnikAfterEvent(currIdx, ACNIK_EVT_ON_BLUR);
}


function acnikOnKeyDown(evt) {
	
	if (!evt) evt = event;
	var kc = evt.keyCode;
	
	if(kc == 9 || kc == 13 || kc == 27) {
		acnikObjShowHidden();
	}
	
	var targetObj = (evt.target) ? evt.target : evt.srcElement;
	var currIdx = -1;
	for(var i = 0; i < acnikObjs.length; i++) {
		if(acnikObjs[i][IDX_FIELD] == targetObj) {
			currIdx = i;
			break;
		}
	}
	
	acnikAfterEvent(currIdx, ACNIK_EVT_ON_KEY_DOWN);
}


function acnikOnKeyUp(evt) {
	if (!evt) evt = event;
	var kc = evt.keyCode;
	var targetObj = (evt.target) ? evt.target : evt.srcElement;
	
	var currIdx = -1;
	for(var i = 0; i < acnikObjs.length; i++) {
		if(acnikObjs[i][IDX_FIELD] == targetObj) {
			currIdx = i;
			break;
		}
	}
	
	if(currIdx < 0) return true;
	
	switch (kc){
		case 9:
			break;
		case 13:
			acnikObjSetValue(currIdx);
			acnikObjHideDrops();
			break;
		case 27:
			acnikObjSetValue(currIdx);
			acnikObjHideDrops();
			break;
		case 37:
			break;
		case 38:
			acnikObjShowHidden();
			acnikObjHideDrops();
			acnikObjDecOffset(currIdx);
			acnikObjShowDrop(currIdx, true, true);
			acnikSetCaret(targetObj, targetObj.value.length);
			break;
		case 39:
			break;
		case 40:
			acnikObjShowHidden();
			acnikObjHideDrops();
			acnikObjIncOffset(currIdx);
			acnikObjShowDrop(currIdx, true, true);
			acnikSetCaret(targetObj, targetObj.value.length);
			break;
		default:
			acnikObjShowHidden();
			acnikObjHideDrops();
			acnikResetMachedValues(currIdx);
			acnikGetNearest(currIdx);
			acnikObjShowDrop(currIdx, false, false);
			acnikSetCaret(targetObj, targetObj.value.length);
			break;
	}
	
	acnikAfterEvent(currIdx, ACNIK_EVT_ON_KEY_UP);
	return true;
}

function acnikOnMouseDown(evt) {
	
	if (!evt) evt = window.event;
	var targetObj = (evt.target) ? evt.target : evt.srcElement;
	
	
	if(targetObj && targetObj.id) {
		if(targetObj.id.indexOf('acnik_span_') >= 0) {
			targetObj = targetObj.parentNode;
			if(!targetObj) return;
		}
		
		if(targetObj.getAttribute('IDX') == null) return;
		var currIdx = targetObj.getAttribute('IDX');
		
		if(targetObj.id.indexOf('acnik_') >= 0 && targetObj.id.indexOf('_prev') > 0) {
			acnikObjShowHidden();
			acnikObjHideDrops();
			acnikObjDecOffset(currIdx);
			acnikObjShowDrop(currIdx, true, true);
			if(acnikObjs[currIdx][IDX_FIELD].value)
				acnikSetCaret(	acnikObjs[currIdx][IDX_FIELD], 
								acnikObjs[currIdx][IDX_FIELD].value.length);
			else
				acnikSetCaret(acnikObjs[currIdx][IDX_FIELD], '');
			return;
		}
		
		if(targetObj.id.indexOf('acnik_') >= 0 && targetObj.id.indexOf('_next') > 0) {
			acnikObjShowHidden();
			acnikObjHideDrops();
			acnikObjIncOffset(currIdx);
			acnikObjShowDrop(currIdx, true, true);
			if(acnikObjs[currIdx][IDX_FIELD].value)
				acnikSetCaret(	acnikObjs[currIdx][IDX_FIELD], 
								acnikObjs[currIdx][IDX_FIELD].value.length);
			else
				acnikSetCaret(acnikObjs[currIdx][IDX_FIELD], '');
			return;
		}
		
		if(targetObj.getAttribute('IDX_OFFSET') == null) return;
		var offset = targetObj.getAttribute('IDX_OFFSET');
		
		obj = acnikObjs[currIdx][IDX_FIELD];
		
		acnikObjSetOffset(currIdx, offset);
		acnikObjSetValue(currIdx);
		acnikObjShowHidden();
		acnikObjHideDrops();
		acnikSetCaret(obj, obj.value.length);
		acnikAfterEvent(currIdx, ACNIK_EVT_ON_MOUSE_OVER);
	}
	
}


function acnikOnMouseOver(evt) {
	
	if (!evt) evt = window.event;
	var targetObj = (evt.target) ? evt.target : evt.srcElement;
	
	if(targetObj && targetObj.id) {
		if(targetObj == acnikPrevSelection) return;
		
		if(targetObj.id.indexOf('acnik_span_') >= 0) {
			targetObj = targetObj.parentNode;
			if(!targetObj) return;
		}
	
		if(acnikPrevSelection) {
			acnikPrevSelection.className = 'acnikDropElt';
		} 
	
		targetObj.className = 'acnikDropEltSelected';
		acnikPrevSelection = targetObj;
	}
	
}


function acnikAfterEvent(currIdx, acnikEventName){
	
	if(!acnikObjs[currIdx]) return false;
	
	var eventArray = acnikObjs[currIdx][IDX_EVENT];
	
	for(var i = 0; i < eventArray.length;i++){
		if(eventArray[i] && eventArray[i].length == 2){
			if(eventArray[i][0] == acnikEventName){
				eval(eventArray[i][1] + '();');
			}
		}
	}
	
}