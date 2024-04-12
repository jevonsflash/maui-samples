/**
 * Skipped minification because the original files appears to be already minified.
 * Original file: /npm/@editorjs/delimiter@1.4.0/dist/delimiter.umd.js
 *
 * Do NOT use SRI with dynamically generated files! More information: https://www.jsdelivr.com/using-sri-with-dynamic-files
 */
(function(){"use strict";try{if(typeof document<"u"){var e=document.createElement("style");e.appendChild(document.createTextNode('.ce-delimiter{line-height:1.6em;width:100%;text-align:center}.ce-delimiter:before{display:inline-block;content:"***";font-size:30px;line-height:65px;height:30px;letter-spacing:.2em}')),document.head.appendChild(e)}}catch(t){console.error("vite-plugin-css-injected-by-js",t)}})();
(function(i,e){typeof exports=="object"&&typeof module<"u"?module.exports=e():typeof define=="function"&&define.amd?define(e):(i=typeof globalThis<"u"?globalThis:i||self,i.Delimiter=e())})(this,function(){"use strict";const i="",e='<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24"><line x1="6" x2="10" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/><line x1="14" x2="18" y1="12" y2="12" stroke="currentColor" stroke-linecap="round" stroke-width="2"/></svg>';/**
 * Delimiter Block for the Editor.js.
 *
 * @author CodeX (team@ifmo.su)
 * @copyright CodeX 2018
 * @license The MIT License (MIT)
 * @version 2.0.0
 */class r{static get isReadOnlySupported(){return!0}static get contentless(){return!0}constructor({data:t,config:o,api:n}){this.api=n,this._CSS={block:this.api.styles.block,wrapper:"ce-delimiter"},this._data={},this._element=this.drawView(),this.data=t}drawView(){let t=document.createElement("DIV");return t.classList.add(this._CSS.wrapper,this._CSS.block),t}render(){return this._element}save(t){return{}}static get toolbox(){return{icon:e,title:"Delimiter"}}}return r});
