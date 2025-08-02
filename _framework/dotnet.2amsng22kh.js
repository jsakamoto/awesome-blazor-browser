//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

var e=!1;const t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,6,64,25,11,11])),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),n=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),r=Symbol.for("wasm promise_control");function i(e,t){let o=null;const n=new Promise((function(n,r){o={isDone:!1,promise:null,resolve:t=>{o.isDone||(o.isDone=!0,n(t),e&&e())},reject:e=>{o.isDone||(o.isDone=!0,r(e),t&&t())}}}));o.promise=n;const i=n;return i[r]=o,{promise:i,promise_control:o}}function s(e){return e[r]}function a(e){e&&function(e){return void 0!==e[r]}(e)||Be(!1,"Promise is not controllable")}const l="__mono_message__",c=["debug","log","trace","warn","info","error"],d="MONO_WASM: ";let u,f,m,g,p,h;function w(e){g=e}function b(e){if(Me.diagnosticTracing){const t="function"==typeof e?e():e;console.debug(d+t)}}function y(e,...t){console.info(d+e,...t)}function v(e,...t){console.info(e,...t)}function E(e,...t){console.warn(d+e,...t)}function _(e,...t){if(t&&t.length>0&&t[0]&&"object"==typeof t[0]){if(t[0].silent)return;if(t[0].toString)return void console.error(d+e,t[0].toString())}console.error(d+e,...t)}function T(e,t,o){return function(...n){try{let r=n[0];if(void 0===r)r="undefined";else if(null===r)r="null";else if("function"==typeof r)r=r.toString();else if("string"!=typeof r)try{r=JSON.stringify(r)}catch(e){r=r.toString()}t(o?JSON.stringify({method:e,payload:r,arguments:n.slice(1)}):[e+r,...n.slice(1)])}catch(e){m.error(`proxyConsole failed: ${e}`)}}}function R(e,t,o){f=t,g=e,m={...t};const n=`${o}/console`.replace("https://","wss://").replace("http://","ws://");u=new WebSocket(n),u.addEventListener("error",A),u.addEventListener("close",S),function(){for(const e of c)f[e]=T(`console.${e}`,j,!0)}()}function x(e){let t=30;const o=()=>{u?0==u.bufferedAmount||0==t?(e&&v(e),function(){for(const e of c)f[e]=T(`console.${e}`,m.log,!1)}(),u.removeEventListener("error",A),u.removeEventListener("close",S),u.close(1e3,e),u=void 0):(t--,globalThis.setTimeout(o,100)):e&&m&&m.log(e)};o()}function j(e){u&&u.readyState===WebSocket.OPEN?u.send(e):m.log(e)}function A(e){m.error(`[${g}] proxy console websocket error: ${e}`,e)}function S(e){m.debug(`[${g}] proxy console websocket closed: ${e}`,e)}function D(){Me.preferredIcuAsset=O(Me.config);let e="invariant"==Me.config.globalizationMode;if(!e)if(Me.preferredIcuAsset)Me.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===Me.config.globalizationMode||"all"===Me.config.globalizationMode||"sharded"===Me.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}Me.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,Me.preferredIcuAsset=null}const t="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",o=Me.config.environmentVariables;if(void 0===o[t]&&e&&(o[t]="1"),void 0===o.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(o.TZ=e)}catch(e){y("failed to detect timezone, will fallback to UTC")}}function O(e){var t;if((null===(t=e.resources)||void 0===t?void 0:t.icu)&&"invariant"!=e.globalizationMode){const t=e.applicationCulture||(Ie?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale),o=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(o.length>=1)return o[0].name}else t&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const t=e.split("-")[0];return"en"===t||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(t)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(t)):n="icudt.dat";if(n)for(let e=0;e<o.length;e++){const t=o[e];if(t.virtualPath===n)return t.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const C=class{constructor(e){this.url=e}toString(){return this.url}};async function I(e,t){try{const o="function"==typeof globalThis.fetch;if(Se){const n=e.startsWith("file://");if(!n&&o)return globalThis.fetch(e,t||{credentials:"same-origin"});p||(h=Ne.require("url"),p=Ne.require("fs")),n&&(e=h.fileURLToPath(e));const r=await p.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>r,json:()=>JSON.parse(r),text:()=>{throw new Error("NotImplementedException")}}}if(o)return globalThis.fetch(e,t||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(t){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t},text:()=>{throw t}}}throw new Error("No fetch implementation available")}function L(e){return"string"!=typeof e&&Be(!1,"url must be a string"),!U(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}const k=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,M=/[a-zA-Z]:[\\/]/;function U(e){return Se||Le?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||M.test(e):k.test(e)}let P,N=0;const $=[],z=[],W=new Map,F={"js-module-threads":!0,"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},B={...F,"js-module-library-initializer":!0},V={...F,dotnetwasm:!0,heap:!0,manifest:!0},q={...B,manifest:!0},H={...B,dotnetwasm:!0},J={dotnetwasm:!0,symbols:!0},Z={...B,dotnetwasm:!0,symbols:!0},Q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=Me.preferredIcuAsset)}function K(e,t,o){null!=t||(t=[]),Be(1==t.length,`Expect to have one ${o} asset in resources`);const n=t[0];return n.behavior=o,X(n),e.push(n),n}function X(e){V[e.behavior]&&W.set(e.behavior,e)}function Y(e){Be(V[e],`Unknown single asset behavior ${e}`);const t=W.get(e);if(t&&!t.resolvedUrl)if(t.resolvedUrl=Me.locateFile(t.name),F[t.behavior]){const e=ge(t);e?("string"!=typeof e&&Be(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),t.resolvedUrl=e):t.resolvedUrl=ce(t.resolvedUrl,t.behavior)}else if("dotnetwasm"!==t.behavior)throw new Error(`Unknown single asset behavior ${e}`);return t}function ee(e){const t=Y(e);return Be(t,`Single asset for ${e} not found`),t}let te=!1;async function oe(){if(!te){te=!0,Me.diagnosticTracing&&b("mono_download_assets");try{const e=[],t=[],o=(e,t)=>{!Z[e.behavior]&&G(e)&&Me.expected_instantiated_assets_count++,!H[e.behavior]&&G(e)&&(Me.expected_downloaded_assets_count++,t.push(se(e)))};for(const t of $)o(t,e);for(const e of z)o(e,t);Me.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...t]).then((()=>{Me.allDownloadsFinished.promise_control.resolve()})).catch((e=>{throw Me.err("Error in mono_download_assets: "+e),Xe(1,e),e})),await Me.runtimeModuleLoaded.promise;const n=async e=>{const t=await e;if(t.buffer){if(!Z[t.behavior]){t.buffer&&"object"==typeof t.buffer||Be(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof t.resolvedUrl&&Be(!1,"resolvedUrl must be string");const e=t.resolvedUrl,o=await t.buffer,n=new Uint8Array(o);pe(t),await ke.beforeOnRuntimeInitialized.promise,ke.instantiate_asset(t,e,n)}}else J[t.behavior]?("symbols"===t.behavior&&(await ke.instantiate_symbols_asset(t),pe(t)),J[t.behavior]&&++Me.actual_downloaded_assets_count):(t.isOptional||Be(!1,"Expected asset to have the downloaded buffer"),!H[t.behavior]&&G(t)&&Me.expected_downloaded_assets_count--,!Z[t.behavior]&&G(t)&&Me.expected_instantiated_assets_count--)},r=[],i=[];for(const t of e)r.push(n(t));for(const e of t)i.push(n(e));Promise.all(r).then((()=>{Ce||ke.coreAssetsInMemory.promise_control.resolve()})).catch((e=>{throw Me.err("Error in mono_download_assets: "+e),Xe(1,e),e})),Promise.all(i).then((async()=>{Ce||(await ke.coreAssetsInMemory.promise,ke.allAssetsInMemory.promise_control.resolve())})).catch((e=>{throw Me.err("Error in mono_download_assets: "+e),Xe(1,e),e}))}catch(e){throw Me.err("Error in mono_download_assets: "+e),e}}}let ne=!1;function re(){if(ne)return;ne=!0;const e=Me.config,t=[];if(e.assets)for(const t of e.assets)"object"!=typeof t&&Be(!1,`asset must be object, it was ${typeof t} : ${t}`),"string"!=typeof t.behavior&&Be(!1,"asset behavior must be known string"),"string"!=typeof t.name&&Be(!1,"asset name must be string"),t.resolvedUrl&&"string"!=typeof t.resolvedUrl&&Be(!1,"asset resolvedUrl could be string"),t.hash&&"string"!=typeof t.hash&&Be(!1,"asset resolvedUrl could be string"),t.pendingDownload&&"object"!=typeof t.pendingDownload&&Be(!1,"asset pendingDownload could be object"),t.isCore?$.push(t):z.push(t),X(t);else if(e.resources){const o=e.resources;o.wasmNative||Be(!1,"resources.wasmNative must be defined"),o.jsModuleNative||Be(!1,"resources.jsModuleNative must be defined"),o.jsModuleRuntime||Be(!1,"resources.jsModuleRuntime must be defined"),K(z,o.wasmNative,"dotnetwasm"),K(t,o.jsModuleNative,"js-module-native"),K(t,o.jsModuleRuntime,"js-module-runtime"),o.jsModuleDiagnostics&&K(t,o.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,t,o)=>{const n=e;n.behavior=t,o?(n.isCore=!0,$.push(n)):z.push(n)};if(o.coreAssembly)for(let e=0;e<o.coreAssembly.length;e++)n(o.coreAssembly[e],"assembly",!0);if(o.assembly)for(let e=0;e<o.assembly.length;e++)n(o.assembly[e],"assembly",!o.coreAssembly);if(0!=e.debugLevel&&Me.isDebuggingSupported()){if(o.corePdb)for(let e=0;e<o.corePdb.length;e++)n(o.corePdb[e],"pdb",!0);if(o.pdb)for(let e=0;e<o.pdb.length;e++)n(o.pdb[e],"pdb",!o.corePdb)}if(e.loadAllSatelliteResources&&o.satelliteResources)for(const e in o.satelliteResources)for(let t=0;t<o.satelliteResources[e].length;t++){const r=o.satelliteResources[e][t];r.culture=e,n(r,"resource",!o.coreAssembly)}if(o.coreVfs)for(let e=0;e<o.coreVfs.length;e++)n(o.coreVfs[e],"vfs",!0);if(o.vfs)for(let e=0;e<o.vfs.length;e++)n(o.vfs[e],"vfs",!o.coreVfs);const r=O(e);if(r&&o.icu)for(let e=0;e<o.icu.length;e++){const t=o.icu[e];t.name===r&&n(t,"icu",!1)}if(o.wasmSymbols)for(let e=0;e<o.wasmSymbols.length;e++)n(o.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let t=0;t<e.appsettings.length;t++){const o=e.appsettings[t],n=he(o);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||z.push({name:o,behavior:"vfs",noCache:!0,useCredentials:!0})}e.assets=[...$,...z,...t]}async function ie(e){const t=await se(e);return await t.pendingDownloadInternal.response,t.buffer}async function se(e){try{return await ae(e)}catch(t){if(!Me.enableDownloadRetry)throw t;if(Le||Se)throw t;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw t;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw t;if(t&&404==t.status)throw t;e.pendingDownloadInternal=void 0,await Me.allDownloadsQueued.promise;try{return Me.diagnosticTracing&&b(`Retrying download '${e.name}'`),await ae(e)}catch(t){return e.pendingDownloadInternal=void 0,await new Promise((e=>globalThis.setTimeout(e,100))),Me.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await ae(e)}}}async function ae(e){for(;P;)await P.promise;try{++N,N==Me.maxParallelDownloads&&(Me.diagnosticTracing&&b("Throttling further parallel downloads"),P=i());const t=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const t=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>t,json:()=>JSON.parse(new TextDecoder("utf-8").decode(t)),text:()=>{throw new Error("NotImplementedException")},headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const t=e.loadRemote&&Me.config.remoteSources?Me.config.remoteSources:[""];let o;for(let n of t){n=n.trim(),"./"===n&&(n="");const t=le(e,n);e.name===t?Me.diagnosticTracing&&b(`Attempting to download '${t}'`):Me.diagnosticTracing&&b(`Attempting to download '${t}' for ${e.name}`);try{e.resolvedUrl=t;const n=fe(e);if(e.pendingDownloadInternal=n,o=await n.response,!o||!o.ok)continue;return o}catch(e){o||(o={ok:!1,url:t,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&Me.config.ignorePdbLoadErrors;if(o||Be(!1,`Response undefined ${e.name}`),!n){const t=new Error(`download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`);throw t.status=o.status,t}y(`optional download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`)}(e);return t?(J[e.behavior]||(e.buffer=await t.arrayBuffer(),++Me.actual_downloaded_assets_count),e):e}finally{if(--N,P&&N==Me.maxParallelDownloads-1){Me.diagnosticTracing&&b("Resuming more parallel downloads");const e=P;P=void 0,e.promise_control.resolve()}}}function le(e,t){let o;return null==t&&Be(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?o=e.resolvedUrl:(o=""===t?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:t+e.name,o=ce(Me.locateFile(o),e.behavior)),o&&"string"==typeof o||Be(!1,"attemptUrl need to be path or url string"),o}function ce(e,t){return Me.modulesUniqueQuery&&q[t]&&(e+=Me.modulesUniqueQuery),e}let de=0;const ue=new Set;function fe(e){try{e.resolvedUrl||Be(!1,"Request's resolvedUrl must be set");const t=function(e){let t=e.resolvedUrl;if(Me.loadBootResource){const o=ge(e);if(o instanceof Promise)return o;"string"==typeof o&&(t=o)}const o={};return Me.config.disableNoCacheFetch||(o.cache="no-cache"),e.useCredentials?o.credentials="include":!Me.config.disableIntegrityCheck&&e.hash&&(o.integrity=e.hash),Me.fetch_like(t,o)}(e),o={name:e.name,url:e.resolvedUrl,response:t};return ue.add(e.name),o.response.then((()=>{"assembly"==e.behavior&&Me.loadedAssemblies.push(e.name),de++,Me.onDownloadResourceProgress&&Me.onDownloadResourceProgress(de,ue.size)})),o}catch(t){const o={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(o)}}}const me={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs","js-module-threads":"dotnetjs"};function ge(e){var t;if(Me.loadBootResource){const o=null!==(t=e.hash)&&void 0!==t?t:"",n=e.resolvedUrl,r=me[e.behavior];if(r){const t=Me.loadBootResource(r,e.name,n,o,e.behavior);return"string"==typeof t?L(t):t}}}function pe(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function he(e){let t=e.lastIndexOf("/");return t>=0&&t++,e.substring(t)}async function we(e){e&&await Promise.all((null!=e?e:[]).map((e=>async function(e){try{const t=e.name;if(!e.moduleExports){const o=ce(Me.locateFile(t),"js-module-library-initializer");Me.diagnosticTracing&&b(`Attempting to import '${o}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */o)}Me.libraryInitializers.push({scriptName:t,exports:e.moduleExports})}catch(t){E(`Failed to import library initializer '${e}': ${t}`)}}(e))))}async function be(e,t){if(!Me.libraryInitializers)return;const o=[];for(let n=0;n<Me.libraryInitializers.length;n++){const r=Me.libraryInitializers[n];r.exports[e]&&o.push(ye(r.scriptName,e,(()=>r.exports[e](...t))))}await Promise.all(o)}async function ye(e,t,o){try{await o()}catch(o){throw E(`Failed to invoke '${t}' on library initializer '${e}': ${o}`),Xe(1,o),o}}function ve(e,t){if(e===t)return e;const o={...t};return void 0!==o.assets&&o.assets!==e.assets&&(o.assets=[...e.assets||[],...o.assets||[]]),void 0!==o.resources&&(o.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},o.resources)),void 0!==o.environmentVariables&&(o.environmentVariables={...e.environmentVariables||{},...o.environmentVariables||{}}),void 0!==o.runtimeOptions&&o.runtimeOptions!==e.runtimeOptions&&(o.runtimeOptions=[...e.runtimeOptions||[],...o.runtimeOptions||[]]),Object.assign(e,o)}function Ee(e,t){if(e===t)return e;const o={...t};return o.config&&(e.config||(e.config={}),o.config=ve(e.config,o.config)),Object.assign(e,o)}function _e(e,t){if(e===t)return e;const o={...t};return void 0!==o.assembly&&(o.assembly=[...e.assembly||[],...o.assembly||[]]),void 0!==o.lazyAssembly&&(o.lazyAssembly=[...e.lazyAssembly||[],...o.lazyAssembly||[]]),void 0!==o.pdb&&(o.pdb=[...e.pdb||[],...o.pdb||[]]),void 0!==o.jsModuleWorker&&(o.jsModuleWorker=[...e.jsModuleWorker||[],...o.jsModuleWorker||[]]),void 0!==o.jsModuleNative&&(o.jsModuleNative=[...e.jsModuleNative||[],...o.jsModuleNative||[]]),void 0!==o.jsModuleDiagnostics&&(o.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...o.jsModuleDiagnostics||[]]),void 0!==o.jsModuleRuntime&&(o.jsModuleRuntime=[...e.jsModuleRuntime||[],...o.jsModuleRuntime||[]]),void 0!==o.wasmSymbols&&(o.wasmSymbols=[...e.wasmSymbols||[],...o.wasmSymbols||[]]),void 0!==o.wasmNative&&(o.wasmNative=[...e.wasmNative||[],...o.wasmNative||[]]),void 0!==o.icu&&(o.icu=[...e.icu||[],...o.icu||[]]),void 0!==o.satelliteResources&&(o.satelliteResources=function(e,t){if(e===t)return e;for(const o in t)e[o]=[...e[o]||[],...t[o]||[]];return e}(e.satelliteResources||{},o.satelliteResources||{})),void 0!==o.modulesAfterConfigLoaded&&(o.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...o.modulesAfterConfigLoaded||[]]),void 0!==o.modulesAfterRuntimeReady&&(o.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...o.modulesAfterRuntimeReady||[]]),void 0!==o.extensions&&(o.extensions={...e.extensions||{},...o.extensions||{}}),void 0!==o.vfs&&(o.vfs=[...e.vfs||[],...o.vfs||[]]),Object.assign(e,o)}function Te(){var e,t;const o=Me.config;if(o.environmentVariables=o.environmentVariables||{},o.runtimeOptions=o.runtimeOptions||[],o.resources=o.resources||{assembly:[],jsModuleNative:[],jsModuleWorker:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},o.assets){Me.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const e of o.assets){const t={};switch(e.behavior){case"assembly":t.assembly=[e];break;case"pdb":t.pdb=[e];break;case"resource":t.satelliteResources={},t.satelliteResources[e.culture]=[e];break;case"icu":t.icu=[e];break;case"symbols":t.wasmSymbols=[e];break;case"vfs":t.vfs=[e];break;case"dotnetwasm":t.wasmNative=[e];break;case"js-module-threads":t.jsModuleWorker=[e];break;case"js-module-runtime":t.jsModuleRuntime=[e];break;case"js-module-native":t.jsModuleNative=[e];break;case"js-module-diagnostics":t.jsModuleDiagnostics=[e];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${e.behavior} of asset ${e.name}`)}_e(o.resources,t)}}o.debugLevel,o.applicationEnvironment||(o.applicationEnvironment="Production"),o.applicationCulture&&(o.environmentVariables.LANG=`${o.applicationCulture}.UTF-8`),0!==o.debugLevel&&(null===(t=null===(e=globalThis.window)||void 0===e?void 0:e.document)||void 0===t?void 0:t.querySelector("script[src*='aspnetcore-browser-refresh']"))&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES||(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES="debug"),o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS||(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS="true")),ke.diagnosticTracing=Me.diagnosticTracing=!!o.diagnosticTracing,ke.waitForDebugger=o.waitForDebugger,Me.maxParallelDownloads=o.maxParallelDownloads||Me.maxParallelDownloads,Me.enableDownloadRetry=void 0!==o.enableDownloadRetry?o.enableDownloadRetry:Me.enableDownloadRetry}let Re=!1;async function xe(e){var t;if(Re)return void await Me.afterConfigLoaded.promise;let o;try{if(e.configSrc||Me.config&&0!==Object.keys(Me.config).length&&(Me.config.assets||Me.config.resources)||(e.configSrc="dotnet.boot.js"),o=e.configSrc,Re=!0,o&&(Me.diagnosticTracing&&b("mono_wasm_load_config"),await async function(e){const t=e.configSrc,o=Me.locateFile(t);let n=null;void 0!==Me.loadBootResource&&(n=Me.loadBootResource("manifest",t,o,"","manifest"));let r,i=null;if(n)if("string"==typeof n)n.includes(".json")?(i=await s(L(n)),r=await Ae(i)):r=(await import(L(n))).config;else{const e=await n;"function"==typeof e.json?(i=e,r=await Ae(i)):r=e.config}else o.includes(".json")?(i=await s(ce(o,"manifest")),r=await Ae(i)):r=(await import(ce(o,"manifest"))).config;function s(e){return Me.fetch_like(e,{method:"GET",credentials:"include",cache:"no-cache"})}Me.config.applicationEnvironment&&(r.applicationEnvironment=Me.config.applicationEnvironment),ve(Me.config,r)}(e)),Te(),await we(null===(t=Me.config.resources)||void 0===t?void 0:t.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[Me.config]),e.onConfigLoaded)try{await e.onConfigLoaded(Me.config,Pe),Te()}catch(e){throw _("onConfigLoaded() failed",e),e}Te(),Me.afterConfigLoaded.promise_control.resolve(Me.config)}catch(t){const n=`Failed to load config file ${o} ${t} ${null==t?void 0:t.stack}`;throw Me.config=e.config=Object.assign(Me.config,{message:n,error:t,isError:!0}),Xe(1,new Error(n)),t}}function je(){return!!globalThis.navigator&&(Me.isChromium||Me.isFirefox)}async function Ae(e){const t=Me.config,o=await e.json();t.applicationEnvironment||o.applicationEnvironment||(o.applicationEnvironment=e.headers.get("Blazor-Environment")||e.headers.get("DotNet-Environment")||void 0),o.environmentVariables||(o.environmentVariables={});const n=e.headers.get("DOTNET-MODIFIABLE-ASSEMBLIES");n&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES=n);const r=e.headers.get("ASPNETCORE-BROWSER-TOOLS");return r&&(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS=r),o}"function"!=typeof importScripts||globalThis.onmessage||(globalThis.dotnetSidecar=!0);const Se="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,De="function"==typeof importScripts,Oe=De&&"undefined"!=typeof dotnetSidecar,Ce=De&&!Oe,Ie="object"==typeof window||De&&!Se,Le=!Ie&&!Se;let ke={},Me={},Ue={},Pe={},Ne={},$e=!1;const ze={},We={config:ze},Fe={mono:{},binding:{},internal:Ne,module:We,loaderHelpers:Me,runtimeHelpers:ke,diagnosticHelpers:Ue,api:Pe};function Be(e,t){if(e)return;const o="Assert failed: "+("function"==typeof t?t():t),n=new Error(o);_(o,n),ke.nativeAbort(n)}function Ve(){return void 0!==Me.exitCode}function qe(){return ke.runtimeReady&&!Ve()}function He(){Ve()&&Be(!1,`.NET runtime already exited with ${Me.exitCode} ${Me.exitReason}. You can use runtime.runMain() which doesn't exit the runtime.`),ke.runtimeReady||Be(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function Je(){Ie&&(globalThis.addEventListener("unhandledrejection",et),globalThis.addEventListener("error",tt))}let Ze,Qe;function Ge(e){Qe&&Qe(e),Xe(e,Me.exitReason)}function Ke(e){Ze&&Ze(e||Me.exitReason),Xe(1,e||Me.exitReason)}function Xe(t,o){var n,r;const i=o&&"object"==typeof o;t=i&&"number"==typeof o.status?o.status:void 0===t?-1:t;const s=i&&"string"==typeof o.message?o.message:""+o;(o=i?o:ke.ExitStatus?function(e,t){const o=new ke.ExitStatus(e);return o.message=t,o.toString=()=>t,o}(t,s):new Error("Exit with code "+t+" "+s)).status=t,o.message||(o.message=s);const a=""+(o.stack||(new Error).stack);try{Object.defineProperty(o,"stack",{get:()=>a})}catch(e){}const l=!!o.silent;if(o.silent=!0,Ve())Me.diagnosticTracing&&b("mono_exit called after exit");else{try{We.onAbort==Ke&&(We.onAbort=Ze),We.onExit==Ge&&(We.onExit=Qe),Ie&&(globalThis.removeEventListener("unhandledrejection",et),globalThis.removeEventListener("error",tt)),ke.runtimeReady?(ke.jiterpreter_dump_stats&&ke.jiterpreter_dump_stats(!1),0===t&&(null===(n=Me.config)||void 0===n?void 0:n.interopCleanupOnExit)&&ke.forceDisposeProxies(!0,!0),e&&0!==t&&(null===(r=Me.config)||void 0===r||r.dumpThreadsOnNonZeroExit)):(Me.diagnosticTracing&&b(`abort_startup, reason: ${o}`),function(e){Me.allDownloadsQueued.promise_control.reject(e),Me.allDownloadsFinished.promise_control.reject(e),Me.afterConfigLoaded.promise_control.reject(e),Me.wasmCompilePromise.promise_control.reject(e),Me.runtimeModuleLoaded.promise_control.reject(e),ke.dotnetReady&&(ke.dotnetReady.promise_control.reject(e),ke.afterInstantiateWasm.promise_control.reject(e),ke.beforePreInit.promise_control.reject(e),ke.afterPreInit.promise_control.reject(e),ke.afterPreRun.promise_control.reject(e),ke.beforeOnRuntimeInitialized.promise_control.reject(e),ke.afterOnRuntimeInitialized.promise_control.reject(e),ke.afterPostRun.promise_control.reject(e))}(o))}catch(e){E("mono_exit A failed",e)}try{l||(function(e,t){if(0!==e&&t){const e=ke.ExitStatus&&t instanceof ke.ExitStatus?b:_;"string"==typeof t?e(t):(void 0===t.stack&&(t.stack=(new Error).stack+""),t.message?e(ke.stringify_as_error_with_stack?ke.stringify_as_error_with_stack(t.message+"\n"+t.stack):t.message+"\n"+t.stack):e(JSON.stringify(t)))}!Ce&&Me.config&&(Me.config.logExitCode?Me.config.forwardConsoleLogsToWS?x("WASM EXIT "+e):v("WASM EXIT "+e):Me.config.forwardConsoleLogsToWS&&x())}(t,o),function(e){if(Ie&&!Ce&&Me.config&&Me.config.appendElementOnExit&&document){const t=document.createElement("label");t.id="tests_done",0!==e&&(t.style.background="red"),t.innerHTML=""+e,document.body.appendChild(t)}}(t))}catch(e){E("mono_exit B failed",e)}Me.exitCode=t,Me.exitReason||(Me.exitReason=o),!Ce&&ke.runtimeReady&&We.runtimeKeepalivePop()}if(Me.config&&Me.config.asyncFlushOnExit&&0===t)throw(async()=>{try{await async function(){try{const e=await import(/*! webpackIgnore: true */"process"),t=e=>new Promise(((t,o)=>{e.on("error",o),e.end("","utf8",t)})),o=t(e.stderr),n=t(e.stdout);let r;const i=new Promise((e=>{r=setTimeout((()=>e("timeout")),1e3)}));await Promise.race([Promise.all([n,o]),i]),clearTimeout(r)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(t,o)}})(),o;Ye(t,o)}function Ye(e,t){if(ke.runtimeReady&&ke.nativeExit)try{ke.nativeExit(e)}catch(e){!ke.ExitStatus||e instanceof ke.ExitStatus||E("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!Ie)throw Se&&Ne.process?Ne.process.exit(e):ke.quit&&ke.quit(e,t),t}function et(e){ot(e,e.reason,"rejection")}function tt(e){ot(e,e.error,"error")}function ot(e,t,o){e.preventDefault();try{t||(t=new Error("Unhandled "+o)),void 0===t.stack&&(t.stack=(new Error).stack),t.stack=t.stack+"",t.silent||(_("Unhandled error:",t),Xe(1,t))}catch(e){}}!function(e){if($e)throw new Error("Loader module already loaded");$e=!0,ke=e.runtimeHelpers,Me=e.loaderHelpers,Ue=e.diagnosticHelpers,Pe=e.api,Ne=e.internal,Object.assign(Pe,{INTERNAL:Ne,invokeLibraryInitializers:be}),Object.assign(e.module,{config:ve(ze,{environmentVariables:{}})});const r={mono_wasm_bindings_is_ready:!1,config:e.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"75972a5ba730bdaf7cf3a34f528ab0f5c7f05183",config:e.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:i(),allDownloadsQueued:i(),allDownloadsFinished:i(),wasmCompilePromise:i(),runtimeModuleLoaded:i(),loadingWorkers:i(),is_exited:Ve,is_runtime_running:qe,assert_runtime_running:He,mono_exit:Xe,createPromiseController:i,getPromiseController:s,assertIsControllablePromise:a,mono_download_assets:oe,resolve_single_asset_path:ee,setup_proxy_console:R,set_thread_prefix:w,installUnhandledErrorHandler:Je,retrieve_asset_download:ie,invokeLibraryInitializers:be,isDebuggingSupported:je,exceptions:t,simd:n,relaxedSimd:o};Object.assign(ke,r),Object.assign(Me,l)}(Fe);let nt,rt,it,st=!1,at=!1;async function lt(e){if(!at){if(at=!0,Ie&&Me.config.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&R("main",globalThis.console,globalThis.location.origin),We||Be(!1,"Null moduleConfig"),Me.config||Be(!1,"Null moduleConfig.config"),"function"==typeof e){const t=e(Fe.api);if(t.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(We,t),Ee(We,t)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");Ee(We,e)}await async function(e){if(Se){const e=await import(/*! webpackIgnore: true */"process"),t=14;if(e.versions.node.split(".")[0]<t)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${t}. See also https://aka.ms/dotnet-wasm-features`)}const t=/*! webpackIgnore: true */import.meta.url,o=t.indexOf("?");var n;if(o>0&&(Me.modulesUniqueQuery=t.substring(o)),Me.scriptUrl=t.replace(/\\/g,"/").replace(/[?#].*/,""),Me.scriptDirectory=(n=Me.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",Me.locateFile=e=>"URL"in globalThis&&globalThis.URL!==C?new URL(e,Me.scriptDirectory).toString():U(e)?e:Me.scriptDirectory+e,Me.fetch_like=I,Me.out=console.log,Me.err=console.error,Me.onDownloadResourceProgress=e.onDownloadResourceProgress,Ie&&globalThis.navigator){const e=globalThis.navigator,t=e.userAgentData&&e.userAgentData.brands;t&&t.length>0?Me.isChromium=t.some((e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand)):e.userAgent&&(Me.isChromium=e.userAgent.includes("Chrome"),Me.isFirefox=e.userAgent.includes("Firefox"))}Ne.require=Se?await import(/*! webpackIgnore: true */"module").then((e=>e.createRequire(/*! webpackIgnore: true */import.meta.url))):Promise.resolve((()=>{throw new Error("require not supported")})),void 0===globalThis.URL&&(globalThis.URL=C)}(We)}}async function ct(e){return await lt(e),Ze=We.onAbort,Qe=We.onExit,We.onAbort=Ke,We.onExit=Ge,We.ENVIRONMENT_IS_PTHREAD?async function(){(function(){const e=new MessageChannel,t=e.port1,o=e.port2;t.addEventListener("message",(e=>{var n,r;n=JSON.parse(e.data.config),r=JSON.parse(e.data.monoThreadInfo),st?Me.diagnosticTracing&&b("mono config already received"):(ve(Me.config,n),ke.monoThreadInfo=r,Te(),Me.diagnosticTracing&&b("mono config received"),st=!0,Me.afterConfigLoaded.promise_control.resolve(Me.config),Ie&&n.forwardConsoleLogsToWS&&void 0!==globalThis.WebSocket&&Me.setup_proxy_console("worker-idle",console,globalThis.location.origin)),t.close(),o.close()}),{once:!0}),t.start(),self.postMessage({[l]:{monoCmd:"preload",port:o}},[o])})(),await Me.afterConfigLoaded.promise,function(){const e=Me.config;e.assets||Be(!1,"config.assets must be defined");for(const t of e.assets)X(t),Q[t.behavior]&&z.push(t)}(),setTimeout((async()=>{try{await oe()}catch(e){Xe(1,e)}}),0);const e=dt(),t=await Promise.all(e);return await ut(t),We}():async function(){var e;await xe(We),re();const t=dt();(async function(){try{const e=ee("dotnetwasm");await se(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||Be(!1,"Can't load dotnet.native.wasm");const t=await e.pendingDownloadInternal.response,o=t.headers&&t.headers.get?t.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===o)n=await WebAssembly.compileStreaming(t);else{Ie&&"application/wasm"!==o&&E('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await t.arrayBuffer();Me.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Le?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,Me.wasmCompilePromise.promise_control.resolve(n)}catch(e){Me.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout((async()=>{try{D(),await oe()}catch(e){Xe(1,e)}}),0);const o=await Promise.all(t);return await ut(o),await ke.dotnetReady.promise,await we(null===(e=Me.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Pe}()}function dt(){const e=ee("js-module-runtime"),t=ee("js-module-native");if(nt&&rt)return[nt,rt,it];"object"==typeof e.moduleExports?nt=e.moduleExports:(Me.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),nt=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof t.moduleExports?rt=t.moduleExports:(Me.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),rt=import(/*! webpackIgnore: true */t.resolvedUrl));const o=Y("js-module-diagnostics");return o&&("object"==typeof o.moduleExports?it=o.moduleExports:(Me.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),it=import(/*! webpackIgnore: true */o.resolvedUrl))),[nt,rt,it]}async function ut(e){const{initializeExports:t,initializeReplacements:o,configureRuntimeStartup:n,configureEmscriptenStartup:r,configureWorkerStartup:i,setRuntimeGlobals:s,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];s(Fe),t(Fe),c&&c.setRuntimeGlobals(Fe),await n(We),Me.runtimeModuleLoaded.promise_control.resolve(),l((e=>(Object.assign(We,{ready:e.ready,__dotnet_runtime:{initializeReplacements:o,configureEmscriptenStartup:r,configureWorkerStartup:i,passEmscriptenInternals:a}}),We))).catch((e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize. See also https://aka.ms/dotnet-wasm-features");throw e}))}const ft=new class{withModuleConfig(e){try{return Ee(We,e),this}catch(e){throw Xe(1,e),e}}withOnConfigLoaded(e){try{return Ee(We,{onConfigLoaded:e}),this}catch(e){throw Xe(1,e),e}}withConsoleForwarding(){try{return ve(ze,{forwardConsoleLogsToWS:!0}),this}catch(e){throw Xe(1,e),e}}withExitOnUnhandledError(){try{return ve(ze,{exitOnUnhandledError:!0}),Je(),this}catch(e){throw Xe(1,e),e}}withAsyncFlushOnExit(){try{return ve(ze,{asyncFlushOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withExitCodeLogging(){try{return ve(ze,{logExitCode:!0}),this}catch(e){throw Xe(1,e),e}}withElementOnExit(){try{return ve(ze,{appendElementOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withInteropCleanupOnExit(){try{return ve(ze,{interopCleanupOnExit:!0}),this}catch(e){throw Xe(1,e),e}}withDumpThreadsOnNonZeroExit(){try{return ve(ze,{dumpThreadsOnNonZeroExit:!0}),this}catch(e){throw Xe(1,e),e}}withWaitingForDebugger(e){try{return ve(ze,{waitForDebugger:e}),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,t){try{return ve(ze,{interpreterPgo:e,interpreterPgoSaveDelay:t}),ze.runtimeOptions?ze.runtimeOptions.push("--interp-pgo-recording"):ze.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ve(ze,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){try{return e&&"string"==typeof e||Be(!1,"must be file path or URL"),Ee(We,{configSrc:e}),this}catch(e){throw Xe(1,e),e}}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||Be(!1,"must be directory path"),ve(ze,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,t){try{const o={};return o[e]=t,ve(ze,{environmentVariables:o}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||Be(!1,"must be dictionary object"),ve(ze,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&Be(!1,"must be boolean"),ve(ze,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||Be(!1,"must be number"),ve(ze,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ve(ze,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ze.runtimeOptions?ze.runtimeOptions.push(...e):ze.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ve(ze,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ve(ze,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ve(ze,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return Me.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lt(We),await xe(We),re(),D(),oe(),await Me.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await ct(We),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}async run(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}},mt=Xe,gt=ct;Le||"function"==typeof globalThis.URL||Be(!1,"This browser/engine doesn't support URL API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),"function"!=typeof globalThis.BigInt64Array&&Be(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),ft.withConfig(/*json-start*/{
  "mainAssemblyName": "AwesomeBlazorBrowser",
  "resources": {
    "hash": "sha256-QmoGzohSuvctBrOAGW5baSDvT15bSkBv6K+R/jK8Mao=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.lgjamath7j.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.0t78nptbqi.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.dhjd9ygih1.wasm",
        "integrity": "sha256-H2vPtRkKedTq13qT9tiH+Hf+8k2VVWlSicNcqQ04SHg="
      }
    ],
    "coreAssembly": [
      {
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.8zdnlxvtrw.wasm",
        "integrity": "sha256-hC/1UpS3wp5kWbM6iakjw1myFymgsSPphMpkgy6e+jQ="
      },
      {
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.x0ibg7vv5x.wasm",
        "integrity": "sha256-/5MdefXrVMelwZ1XZyid45b/LO7P6Fw2manPR6G3AfI="
      }
    ],
    "assembly": [
      {
        "virtualPath": "AwesomeBlazor.Models.wasm",
        "name": "AwesomeBlazor.Models.4qb5db2sk5.wasm",
        "integrity": "sha256-nCOrsmcJtoIkKpQYIsdL1xabQerZuX2v4dhBOF0dYZM="
      },
      {
        "virtualPath": "AwesomeBlazorBrowser.wasm",
        "name": "AwesomeBlazorBrowser.pgdapkl9bw.wasm",
        "integrity": "sha256-qXvEwYStjog/b50PUPDwqgVuhklKa1+lRq8mq0sz6vs="
      },
      {
        "virtualPath": "Markdig.wasm",
        "name": "Markdig.ato3fn3juu.wasm",
        "integrity": "sha256-XrsZGktyAakARIo+IPOpkBODiKrhtyzTXNj2m8vj390="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.1romjh1llu.wasm",
        "integrity": "sha256-M7JMoAIzf2GJrx49A/1KHOC+UClBXkv8ZceRM/wYjqo="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.7l6rccj2e6.wasm",
        "integrity": "sha256-G0aXLo4Om6h0Yk2Mjn+Nw9bpR8kNTt9HhnE1TWoDY6s="
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.qlng6ofkbw.wasm",
        "integrity": "sha256-gg5BXA4zOpPTK/U2LTkooh3oiKOvJUJROT0hCRO4Ya0="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.i76wihz0te.wasm",
        "integrity": "sha256-bk0dHFISnH9gE/pH9IR+Ic30HQ6Cu/yYcBte6qKhA+M="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.5mlog3zqi1.wasm",
        "integrity": "sha256-k9CM6UFmAYvdg62cXaBanRFpwD5YvbcKS2daUzL8ByE="
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.k8bcwu67ln.wasm",
        "integrity": "sha256-5DuL6Y4+63QYsjnv5AYYKPKPvL0EL2T12Yo1fwIEkKk="
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.fe4layt04y.wasm",
        "integrity": "sha256-eRSmcMFyLh2m9jOcCdtlijsC2icctPxOovl78n3p+K8="
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.3kbvtbnjtc.wasm",
        "integrity": "sha256-vTrX0yS/Qb7KN5C37yJq8DhKD6YEHzw/mqjdEvqfLoY="
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.09wzii4at6.wasm",
        "integrity": "sha256-BChmbXetdx5CdRzkbl/mQrdMOmDVRGXSNFmDs42JOi0="
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.f718bsiqjh.wasm",
        "integrity": "sha256-GzMyhO5eKcpQBqNj053C83IPqbylnI4DY3yleWgXP4M="
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.tt6tisew7o.wasm",
        "integrity": "sha256-LxaiyQab9wW6bDR2Ivx9HstjSHhJwy0zAikuquPkidA="
      },
      {
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.1850p1oagr.wasm",
        "integrity": "sha256-cXefCTuX1HFBYaoREAQwyGczqzm7PjQ60eu5y34sOYg="
      },
      {
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.z9fyfaxqmr.wasm",
        "integrity": "sha256-1+gIW4fyNOU35DYylWKP3x5P46r4DfkR2wgv+vABaTw="
      },
      {
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.r8o53ioqzk.wasm",
        "integrity": "sha256-+3Q/4MXZH84DyadoyIanVgTN1tEmP1ZJd/YHff/wvPQ="
      },
      {
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.98c4jgvjg9.wasm",
        "integrity": "sha256-Fwb2FhH+lUf4kqAiIHc6xoV0XMo1RN6XLjvKBgELE4o="
      },
      {
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.nmy6jie146.wasm",
        "integrity": "sha256-z48IVve5f0h8erVHjLECqRRAH3zVsRyQp4AmML9LahM="
      },
      {
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.t9qo95e80z.wasm",
        "integrity": "sha256-/6fRbk+8n2zsid0XuSQlGn1FQTOUfGd7drwjSBcy5sQ="
      },
      {
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.cidvxfobr7.wasm",
        "integrity": "sha256-3AxvprZSD4L3xxREkRPScONW9o6Pejh9BL/T+tSqZK0="
      },
      {
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.tzoxl3b8k8.wasm",
        "integrity": "sha256-NzIS1HmsZb7iEuUfkL7gRG33RO5eUpTbXVj6R8OJvy4="
      },
      {
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.aqq1se7zwd.wasm",
        "integrity": "sha256-bv6L/Z4CGH6qkb/TpbHEIai+RRmK3Y8lAXY5XvlXrDg="
      },
      {
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.om1oydlwsk.wasm",
        "integrity": "sha256-wNs8yipRBiyjCCc5v+dZpPA+QqFRqOoi13FMrlYIB8s="
      },
      {
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.i5qw1tae92.wasm",
        "integrity": "sha256-ZqFUAiHeabFE+4pDIS/Zs/rPvRFwhfKSQgvbFWIMqQI="
      },
      {
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.zmgxwp8d7r.wasm",
        "integrity": "sha256-e7ZaAajBWdo84IgnL/rsCdpNtYpLU95KjA5/46wRzwA="
      },
      {
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.9oqugbg9cz.wasm",
        "integrity": "sha256-iT4ui+MnS94KNj0siGL79cgCoK6ibAY83h9wT6WeFgY="
      },
      {
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.awfqop73hi.wasm",
        "integrity": "sha256-+h7x2GRJhpcVfNyVzc0RO+JCffHto8vRkepWY5wnUbA="
      },
      {
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.9wjdvfi8x5.wasm",
        "integrity": "sha256-Bx82vbEbNHnkEX7wv84Q7WziEonArL65RkJIpoamR28="
      },
      {
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.li6txpgq7t.wasm",
        "integrity": "sha256-PQvbbK9mj9c3u84q4bgo219Nm1lgfT5J3CeSlzZOjSo="
      },
      {
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.3jamd20seq.wasm",
        "integrity": "sha256-z1OX+g34I64MxLH5R11O/HmSdojhHycRUzSwWHcFJUI="
      },
      {
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.wem6azl8px.wasm",
        "integrity": "sha256-CEOLOtK/mfrkO/Jr4AXIMpTNNyJllDhsajY3qo/76xc="
      },
      {
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.v6oj7tdrxx.wasm",
        "integrity": "sha256-N4zPHh/IC1Z+e9GsMJgJF1pVFu7ExxkxwC2sMpRXInM="
      },
      {
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.whk7wyu1r0.wasm",
        "integrity": "sha256-fJS/eS+B7/uWqSnSG3g4zT/IcCDLpcMfnVc+iyKgDuA="
      },
      {
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.x92khdwf7w.wasm",
        "integrity": "sha256-e/K0vBoNToTJ3jloHIZuozIU2nNaVWaupTl3HTYy0NQ="
      },
      {
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.c2t4mb3qeq.wasm",
        "integrity": "sha256-JP7Rb24y0AscgyTEV+HffS2EkQGirFRtxkc4HgiwbEc="
      },
      {
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.0yut7k3kph.wasm",
        "integrity": "sha256-GFdbSvOcWG6RkK8ED+OgKlil0V79ZC+PO3WrK2yly94="
      },
      {
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.a5b1jyz0r3.wasm",
        "integrity": "sha256-UiY4H+5BPMPkb+jg7AgMZBxpMh6PxbdOrrynYcudGFM="
      },
      {
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.5y1sr66wtt.wasm",
        "integrity": "sha256-I9SU3nH3bvPBd5PU5Y77R5Uu46B6wh8JmcPNgk4bMBk="
      },
      {
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.d1mbzue354.wasm",
        "integrity": "sha256-r+AEWgvQ7LqZxypRSDtTuWb8zvj3aHd1105V9BFfj2A="
      },
      {
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.nop99peyka.wasm",
        "integrity": "sha256-uQi1xUJQ6mX2T93RrPAi6qK2Krv78A1gtVhMNUpq518="
      },
      {
        "virtualPath": "System.wasm",
        "name": "System.cmfj5dol2p.wasm",
        "integrity": "sha256-LPG8ZxMpsYXqNLjsri5f2BjRPSIE/EdfuXkodZwEnGA="
      },
      {
        "virtualPath": "Toolbelt.Web.CssClassInlineBuilder.wasm",
        "name": "Toolbelt.Web.CssClassInlineBuilder.oghibham5h.wasm",
        "integrity": "sha256-6Be1iSAu3Jw7TQYn05vo+aZgIPsd/jtpLMIwMERlHjE="
      }
    ],
    "libraryInitializers": {
      "BlazorWasmPreRendering.Build.lfyg69o9wu.lib.module.js": "sha256-UVy3L6kXaa89JN6tg+EbiLCuVLpkmxOoG6sAi7Bb9uc="
    },
    "modulesAfterConfigLoaded": [
      {
        "name": "../BlazorWasmPreRendering.Build.lfyg69o9wu.lib.module.js"
      }
    ]
  },
  "debugLevel": 0,
  "linkerEnabled": true,
  "globalizationMode": "invariant",
  "extensions": {
    "blazor": {}
  },
  "runtimeConfig": {
    "runtimeOptions": {
      "configProperties": {
        "Microsoft.AspNetCore.Components.Routing.RegexConstraintSupport": false,
        "System.Net.Http.WasmEnableStreamingResponse": true,
        "Microsoft.Extensions.DependencyInjection.VerifyOpenGenericServiceTrimmability": true,
        "System.ComponentModel.DefaultValueAttribute.IsSupported": false,
        "System.ComponentModel.Design.IDesignerHost.IsSupported": false,
        "System.ComponentModel.TypeConverter.EnableUnsafeBinaryFormatterInDesigntimeLicenseContextSerialization": false,
        "System.ComponentModel.TypeDescriptor.IsComObjectDescriptorSupported": false,
        "System.Data.DataSet.XmlSerializationIsSupported": false,
        "System.Diagnostics.Debugger.IsSupported": false,
        "System.Diagnostics.Metrics.Meter.IsSupported": false,
        "System.Diagnostics.Tracing.EventSource.IsSupported": false,
        "System.GC.Server": true,
        "System.Globalization.Invariant": true,
        "System.TimeZoneInfo.Invariant": true,
        "System.Globalization.PredefinedCulturesOnly": true,
        "System.Linq.Enumerable.IsSizeOptimized": true,
        "System.Net.Http.EnableActivityPropagation": false,
        "System.Reflection.Metadata.MetadataUpdater.IsSupported": false,
        "System.Resources.ResourceManager.AllowCustomResourceTypes": false,
        "System.Resources.UseSystemResourceKeys": true,
        "System.Runtime.CompilerServices.RuntimeFeature.IsDynamicCodeSupported": true,
        "System.Runtime.InteropServices.BuiltInComInterop.IsSupported": false,
        "System.Runtime.InteropServices.EnableConsumingManagedCodeFromNativeHosting": false,
        "System.Runtime.InteropServices.EnableCppCLIHostActivation": false,
        "System.Runtime.InteropServices.Marshalling.EnableGeneratedComInterfaceComImportInterop": false,
        "System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization": false,
        "System.StartupHookProvider.IsSupported": false,
        "System.Text.Encoding.EnableUnsafeUTF7Encoding": false,
        "System.Text.Json.JsonSerializer.IsReflectionEnabledByDefault": true,
        "System.Threading.Thread.EnableAutoreleasePool": false,
        "Microsoft.AspNetCore.Components.Endpoints.NavigationManager.DisableThrowNavigationException": false
      }
    }
  }
}/*json-end*/);export{gt as default,ft as dotnet,mt as exit};
//# sourceMappingURL=dotnet.js.map
