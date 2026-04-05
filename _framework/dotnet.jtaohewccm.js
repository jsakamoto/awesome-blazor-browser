//! Licensed to the .NET Foundation under one or more agreements.
//! The .NET Foundation licenses this file to you under the MIT license.

const e=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,4,1,96,0,0,3,2,1,0,10,8,1,6,0,6,64,25,11,11])),t=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,15,1,13,0,65,1,253,15,65,2,253,15,253,128,2,11])),o=async()=>WebAssembly.validate(new Uint8Array([0,97,115,109,1,0,0,0,1,5,1,96,0,1,123,3,2,1,0,10,10,1,8,0,65,0,253,15,253,98,11])),n=Symbol.for("wasm promise_control");function i(e,t){let o=null;const i=new Promise(function(n,i){o={isDone:!1,promise:null,resolve:t=>{o.isDone||(o.isDone=!0,n(t),e&&e())},reject:e=>{o.isDone||(o.isDone=!0,i(e),t&&t())}}});o.promise=i;const r=i;return r[n]=o,{promise:r,promise_control:o}}function r(e){return e[n]}function s(e){e&&function(e){return void 0!==e[n]}(e)||Be(!1,"Promise is not controllable")}const a="__mono_message__",l=["debug","log","trace","warn","info","error"],c="MONO_WASM: ";let d,u,f,m,g,p;function h(e){m=e}function b(e){if(Pe.diagnosticTracing){const t="function"==typeof e?e():e;console.debug(c+t)}}function w(e,...t){console.info(c+e,...t)}function y(e,...t){console.info(e,...t)}function v(e,...t){console.warn(c+e,...t)}function _(e,...t){if(t&&t.length>0&&t[0]&&"object"==typeof t[0]){if(t[0].silent)return;if(t[0].toString)return void console.error(c+e,t[0].toString())}console.error(c+e,...t)}function E(e,t,o){return function(...n){try{let i=n[0];if(void 0===i)i="undefined";else if(null===i)i="null";else if("function"==typeof i)i=i.toString();else if("string"!=typeof i)try{i=JSON.stringify(i)}catch(e){i=i.toString()}t(o?JSON.stringify({method:e,payload:i,arguments:n.slice(1)}):[e+i,...n.slice(1)])}catch(e){f.error(`proxyConsole failed: ${e}`)}}}function R(e,t,o){u=t,m=e,f={...t};const n=`${o}/console`.replace("https://","wss://").replace("http://","ws://");d=new WebSocket(n),d.addEventListener("error",x),d.addEventListener("close",A),function(){for(const e of l)u[e]=E(`console.${e}`,T,!0)}()}function j(e){let t=30;const o=()=>{d?0==d.bufferedAmount||0==t?(e&&y(e),function(){for(const e of l)u[e]=E(`console.${e}`,f.log,!1)}(),d.removeEventListener("error",x),d.removeEventListener("close",A),d.close(1e3,e),d=void 0):(t--,globalThis.setTimeout(o,100)):e&&f&&f.log(e)};o()}function T(e){d&&d.readyState===WebSocket.OPEN?d.send(e):f.log(e)}function x(e){f.error(`[${m}] proxy console websocket error: ${e}`,e)}function A(e){f.debug(`[${m}] proxy console websocket closed: ${e}`,e)}function D(){Pe.preferredIcuAsset=S(Pe.config);let e="invariant"==Pe.config.globalizationMode;if(!e)if(Pe.preferredIcuAsset)Pe.diagnosticTracing&&b("ICU data archive(s) available, disabling invariant mode");else{if("custom"===Pe.config.globalizationMode||"all"===Pe.config.globalizationMode||"sharded"===Pe.config.globalizationMode){const e="invariant globalization mode is inactive and no ICU data archives are available";throw _(`ERROR: ${e}`),new Error(e)}Pe.diagnosticTracing&&b("ICU data archive(s) not available, using invariant globalization mode"),e=!0,Pe.preferredIcuAsset=null}const t="DOTNET_SYSTEM_GLOBALIZATION_INVARIANT",o=Pe.config.environmentVariables;if(void 0===o[t]&&e&&(o[t]="1"),void 0===o.TZ)try{const e=Intl.DateTimeFormat().resolvedOptions().timeZone||null;e&&(o.TZ=e)}catch(e){w("failed to detect timezone, will fallback to UTC")}}function S(e){var t;if((null===(t=e.resources)||void 0===t?void 0:t.icu)&&"invariant"!=e.globalizationMode){const t=e.applicationCulture||(Me?globalThis.navigator&&globalThis.navigator.languages&&globalThis.navigator.languages[0]:Intl.DateTimeFormat().resolvedOptions().locale);e.applicationCulture||(e.applicationCulture=t);const o=e.resources.icu;let n=null;if("custom"===e.globalizationMode){if(o.length>=1)return o[0].name}else t&&"all"!==e.globalizationMode?"sharded"===e.globalizationMode&&(n=function(e){const t=e.split("-")[0];return"en"===t||["fr","fr-FR","it","it-IT","de","de-DE","es","es-ES"].includes(e)?"icudt_EFIGS.dat":["zh","ko","ja"].includes(t)?"icudt_CJK.dat":"icudt_no_CJK.dat"}(t)):n="icudt.dat";if(n)for(let e=0;e<o.length;e++){const t=o[e];if(t.virtualPath===n)return t.name}}return e.globalizationMode="invariant",null}(new Date).valueOf();const k=class{constructor(e){this.url=e}toString(){return this.url}};async function M(e,t){try{const o="function"==typeof globalThis.fetch;if(Ae){const n=e.startsWith("file://");if(!n&&o)return globalThis.fetch(e,t||{credentials:"same-origin"});g||(p=await import(/*! webpackIgnore: true */"url"),g=await import(/*! webpackIgnore: true */"fs")),n&&(e=p.fileURLToPath(e));const i=await g.promises.readFile(e);return{ok:!0,headers:{length:0,get:()=>null},url:e,arrayBuffer:()=>i,json:()=>JSON.parse(i),text:()=>{throw new Error("NotImplementedException")}}}if(o)return globalThis.fetch(e,t||{credentials:"same-origin"});if("function"==typeof read)return{ok:!0,url:e,headers:{length:0,get:()=>null},arrayBuffer:()=>new Uint8Array(read(e,"binary")),json:()=>JSON.parse(read(e,"utf8")),text:()=>read(e,"utf8")}}catch(t){return{ok:!1,url:e,status:500,headers:{length:0,get:()=>null},statusText:"ERR28: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t},text:()=>{throw t}}}throw new Error("No fetch implementation available")}function C(e){return"string"!=typeof e&&Be(!1,"url must be a string"),!P(e)&&0!==e.indexOf("./")&&0!==e.indexOf("../")&&globalThis.URL&&globalThis.document&&globalThis.document.baseURI&&(e=new URL(e,globalThis.document.baseURI).toString()),e}const I=/^[a-zA-Z][a-zA-Z\d+\-.]*?:\/\//,O=/[a-zA-Z]:[\\/]/;function P(e){return Ae||Ce?e.startsWith("/")||e.startsWith("\\")||-1!==e.indexOf("///")||O.test(e):I.test(e)}let U,L=0;const N=[],$=[],z=new Map,W={"js-module-threads":!0,"js-module-runtime":!0,"js-module-dotnet":!0,"js-module-native":!0,"js-module-diagnostics":!0},F={...W,"js-module-library-initializer":!0},B={...W,dotnetwasm:!0,heap:!0,manifest:!0},V={...F,manifest:!0},H={...F,dotnetwasm:!0},J={dotnetwasm:!0,symbols:!0},q={...F,dotnetwasm:!0,symbols:!0},Q={symbols:!0};function G(e){return!("icu"==e.behavior&&e.name!=Pe.preferredIcuAsset)}function Z(e,t,o){null!=t||(t=[]),Be(1==t.length,`Expect to have one ${o} asset in resources`);const n=t[0];return n.behavior=o,K(n),e.push(n),n}function K(e){B[e.behavior]&&z.set(e.behavior,e)}function X(e){Be(B[e],`Unknown single asset behavior ${e}`);const t=z.get(e);if(t&&!t.resolvedUrl)if(t.resolvedUrl=Pe.locateFile(t.name),W[t.behavior]){const e=me(t);e?("string"!=typeof e&&Be(!1,"loadBootResource response for 'dotnetjs' type should be a URL string"),t.resolvedUrl=e):t.resolvedUrl=le(t.resolvedUrl,t.behavior)}else if("dotnetwasm"!==t.behavior)throw new Error(`Unknown single asset behavior ${e}`);return t}function Y(e){const t=X(e);return Be(t,`Single asset for ${e} not found`),t}let ee=!1;async function te(){if(!ee){ee=!0,Pe.diagnosticTracing&&b("mono_download_assets");try{const e=[],t=[],o=(e,t)=>{!q[e.behavior]&&G(e)&&Pe.expected_instantiated_assets_count++,!H[e.behavior]&&G(e)&&(Pe.expected_downloaded_assets_count++,t.push(re(e)))};for(const t of N)o(t,e);for(const e of $)o(e,t);Pe.allDownloadsQueued.promise_control.resolve(),Promise.all([...e,...t]).then(()=>{Pe.allDownloadsFinished.promise_control.resolve()}).catch(e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e}),await Pe.runtimeModuleLoaded.promise;const n=async e=>{const t=await e;if(t.buffer){if(!q[t.behavior]){t.buffer&&"object"==typeof t.buffer||Be(!1,"asset buffer must be array-like or buffer-like or promise of these"),"string"!=typeof t.resolvedUrl&&Be(!1,"resolvedUrl must be string");const e=t.resolvedUrl,o=await t.buffer,n=new Uint8Array(o);ge(t),await Oe.beforeOnRuntimeInitialized.promise,await Oe.afterInstantiateWasm.promise,Oe.instantiate_asset(t,e,n)}}else J[t.behavior]?("symbols"===t.behavior&&(await Oe.instantiate_symbols_asset(t),ge(t)),J[t.behavior]&&++Pe.actual_downloaded_assets_count):(t.isOptional||Be(!1,"Expected asset to have the downloaded buffer"),!H[t.behavior]&&G(t)&&Pe.expected_downloaded_assets_count--,!q[t.behavior]&&G(t)&&Pe.expected_instantiated_assets_count--)},i=[],r=[];for(const t of e)i.push(n(t));for(const e of t)r.push(n(e));Promise.all(i).then(()=>{ke||Oe.coreAssetsInMemory.promise_control.resolve()}).catch(e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e}),Promise.all(r).then(async()=>{ke||(await Oe.coreAssetsInMemory.promise,Oe.allAssetsInMemory.promise_control.resolve())}).catch(e=>{throw Pe.err("Error in mono_download_assets: "+e),Xe(1,e),e})}catch(e){throw Pe.err("Error in mono_download_assets: "+e),e}}}let oe=!1;function ne(){if(oe)return;oe=!0;const e=Pe.config,t=[];if(e.assets)for(const t of e.assets)"object"!=typeof t&&Be(!1,`asset must be object, it was ${typeof t} : ${t}`),"string"!=typeof t.behavior&&Be(!1,"asset behavior must be known string"),"string"!=typeof t.name&&Be(!1,"asset name must be string"),t.resolvedUrl&&"string"!=typeof t.resolvedUrl&&Be(!1,"asset resolvedUrl could be string"),t.hash&&"string"!=typeof t.hash&&Be(!1,"asset resolvedUrl could be string"),t.pendingDownload&&"object"!=typeof t.pendingDownload&&Be(!1,"asset pendingDownload could be object"),t.isCore?N.push(t):$.push(t),K(t);else if(e.resources){const o=e.resources;o.wasmNative||Be(!1,"resources.wasmNative must be defined"),o.jsModuleNative||Be(!1,"resources.jsModuleNative must be defined"),o.jsModuleRuntime||Be(!1,"resources.jsModuleRuntime must be defined"),Z($,o.wasmNative,"dotnetwasm"),Z(t,o.jsModuleNative,"js-module-native"),Z(t,o.jsModuleRuntime,"js-module-runtime"),o.jsModuleDiagnostics&&Z(t,o.jsModuleDiagnostics,"js-module-diagnostics");const n=(e,t,o)=>{const n=e;n.behavior=t,o?(n.isCore=!0,N.push(n)):$.push(n)};if(o.coreAssembly)for(let e=0;e<o.coreAssembly.length;e++)n(o.coreAssembly[e],"assembly",!0);if(o.assembly)for(let e=0;e<o.assembly.length;e++)n(o.assembly[e],"assembly",!o.coreAssembly);if(0!=e.debugLevel&&Pe.isDebuggingSupported()){if(o.corePdb)for(let e=0;e<o.corePdb.length;e++)n(o.corePdb[e],"pdb",!0);if(o.pdb)for(let e=0;e<o.pdb.length;e++)n(o.pdb[e],"pdb",!o.corePdb)}if(e.loadAllSatelliteResources&&o.satelliteResources)for(const e in o.satelliteResources)for(let t=0;t<o.satelliteResources[e].length;t++){const i=o.satelliteResources[e][t];i.culture=e,n(i,"resource",!o.coreAssembly)}if(o.coreVfs)for(let e=0;e<o.coreVfs.length;e++)n(o.coreVfs[e],"vfs",!0);if(o.vfs)for(let e=0;e<o.vfs.length;e++)n(o.vfs[e],"vfs",!o.coreVfs);const i=S(e);if(i&&o.icu)for(let e=0;e<o.icu.length;e++){const t=o.icu[e];t.name===i&&n(t,"icu",!1)}if(o.wasmSymbols)for(let e=0;e<o.wasmSymbols.length;e++)n(o.wasmSymbols[e],"symbols",!1)}if(e.appsettings)for(let t=0;t<e.appsettings.length;t++){const o=e.appsettings[t],n=pe(o);"appsettings.json"!==n&&n!==`appsettings.${e.applicationEnvironment}.json`||$.push({name:o,behavior:"vfs",cache:"no-cache",useCredentials:!0})}e.assets=[...N,...$,...t]}async function ie(e){const t=await re(e);return await t.pendingDownloadInternal.response,t.buffer}async function re(e){try{return await se(e)}catch(t){if(!Pe.enableDownloadRetry)throw t;if(Ce||Ae)throw t;if(e.pendingDownload&&e.pendingDownloadInternal==e.pendingDownload)throw t;if(e.resolvedUrl&&-1!=e.resolvedUrl.indexOf("file://"))throw t;if(t&&404==t.status)throw t;e.pendingDownloadInternal=void 0,await Pe.allDownloadsQueued.promise;try{return Pe.diagnosticTracing&&b(`Retrying download '${e.name}'`),await se(e)}catch(t){return e.pendingDownloadInternal=void 0,await new Promise(e=>globalThis.setTimeout(e,100)),Pe.diagnosticTracing&&b(`Retrying download (2) '${e.name}' after delay`),await se(e)}}}async function se(e){for(;U;)await U.promise;try{++L,L==Pe.maxParallelDownloads&&(Pe.diagnosticTracing&&b("Throttling further parallel downloads"),U=i());const t=await async function(e){if(e.pendingDownload&&(e.pendingDownloadInternal=e.pendingDownload),e.pendingDownloadInternal&&e.pendingDownloadInternal.response)return e.pendingDownloadInternal.response;if(e.buffer){const t=await e.buffer;return e.resolvedUrl||(e.resolvedUrl="undefined://"+e.name),e.pendingDownloadInternal={url:e.resolvedUrl,name:e.name,response:Promise.resolve({ok:!0,arrayBuffer:()=>t,json:()=>JSON.parse(new TextDecoder("utf-8").decode(t)),text:()=>{throw new Error("NotImplementedException")},headers:{get:()=>{}}})},e.pendingDownloadInternal.response}const t=e.loadRemote&&Pe.config.remoteSources?Pe.config.remoteSources:[""];let o;for(let n of t){n=n.trim(),"./"===n&&(n="");const t=ae(e,n);e.name===t?Pe.diagnosticTracing&&b(`Attempting to download '${t}'`):Pe.diagnosticTracing&&b(`Attempting to download '${t}' for ${e.name}`);try{e.resolvedUrl=t;const n=ue(e);if(e.pendingDownloadInternal=n,o=await n.response,!o||!o.ok)continue;return o}catch(e){o||(o={ok:!1,url:t,status:0,statusText:""+e});continue}}const n=e.isOptional||e.name.match(/\.pdb$/)&&Pe.config.ignorePdbLoadErrors;if(o||Be(!1,`Response undefined ${e.name}`),!n){const t=new Error(`download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`);throw t.status=o.status,t}w(`optional download '${o.url}' for ${e.name} failed ${o.status} ${o.statusText}`)}(e);return t?(J[e.behavior]||(e.buffer=await t.arrayBuffer(),++Pe.actual_downloaded_assets_count),e):e}finally{if(--L,U&&L==Pe.maxParallelDownloads-1){Pe.diagnosticTracing&&b("Resuming more parallel downloads");const e=U;U=void 0,e.promise_control.resolve()}}}function ae(e,t){let o;return null==t&&Be(!1,`sourcePrefix must be provided for ${e.name}`),e.resolvedUrl?o=e.resolvedUrl:(o=""===t?"assembly"===e.behavior||"pdb"===e.behavior?e.name:"resource"===e.behavior&&e.culture&&""!==e.culture?`${e.culture}/${e.name}`:e.name:t+e.name,o=le(Pe.locateFile(o),e.behavior)),o&&"string"==typeof o||Be(!1,"attemptUrl need to be path or url string"),o}function le(e,t){return Pe.modulesUniqueQuery&&V[t]&&(e+=Pe.modulesUniqueQuery),e}let ce=0;const de=new Set;function ue(e){try{e.resolvedUrl||Be(!1,"Request's resolvedUrl must be set");const t=function(e){let t=e.resolvedUrl;if(Pe.loadBootResource){const o=me(e);if(o instanceof Promise)return o;"string"==typeof o&&(t=o)}const o={};return e.cache?o.cache=e.cache:Pe.config.disableNoCacheFetch||(o.cache="no-cache"),e.useCredentials?o.credentials="include":!Pe.config.disableIntegrityCheck&&e.hash&&(o.integrity=e.hash),Pe.fetch_like(t,o)}(e),o={name:e.name,url:e.resolvedUrl,response:t};return de.add(e.name),o.response.then(()=>{"assembly"==e.behavior&&Pe.loadedAssemblies.push(e.name),ce++,Pe.onDownloadResourceProgress&&Pe.onDownloadResourceProgress(ce,de.size)}),o}catch(t){const o={ok:!1,url:e.resolvedUrl,status:500,statusText:"ERR29: "+t,arrayBuffer:()=>{throw t},json:()=>{throw t}};return{name:e.name,url:e.resolvedUrl,response:Promise.resolve(o)}}}const fe={resource:"assembly",assembly:"assembly",pdb:"pdb",icu:"globalization",vfs:"configuration",manifest:"manifest",dotnetwasm:"dotnetwasm","js-module-dotnet":"dotnetjs","js-module-native":"dotnetjs","js-module-runtime":"dotnetjs","js-module-threads":"dotnetjs"};function me(e){var t;if(Pe.loadBootResource){const o=null!==(t=e.hash)&&void 0!==t?t:"",n=e.resolvedUrl,i=fe[e.behavior];if(i){const t=Pe.loadBootResource(i,e.name,n,o,e.behavior);return"string"==typeof t?C(t):t}}}function ge(e){e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null}function pe(e){let t=e.lastIndexOf("/");return t>=0&&t++,e.substring(t)}async function he(e){e&&await Promise.all((null!=e?e:[]).map(e=>async function(e){try{const t=e.name;if(!e.moduleExports){const o=le(Pe.locateFile(t),"js-module-library-initializer");Pe.diagnosticTracing&&b(`Attempting to import '${o}' for ${e}`),e.moduleExports=await import(/*! webpackIgnore: true */o)}Pe.libraryInitializers.push({scriptName:t,exports:e.moduleExports})}catch(t){v(`Failed to import library initializer '${e}': ${t}`)}}(e)))}async function be(e,t){if(!Pe.libraryInitializers)return;const o=[];for(let n=0;n<Pe.libraryInitializers.length;n++){const i=Pe.libraryInitializers[n];i.exports[e]&&o.push(we(i.scriptName,e,()=>i.exports[e](...t)))}await Promise.all(o)}async function we(e,t,o){try{await o()}catch(o){throw v(`Failed to invoke '${t}' on library initializer '${e}': ${o}`),Xe(1,o),o}}function ye(e,t){if(e===t)return e;const o={...t};return void 0!==o.assets&&o.assets!==e.assets&&(o.assets=[...e.assets||[],...o.assets||[]]),void 0!==o.resources&&(o.resources=_e(e.resources||{assembly:[],jsModuleNative:[],jsModuleRuntime:[],wasmNative:[]},o.resources)),void 0!==o.environmentVariables&&(o.environmentVariables={...e.environmentVariables||{},...o.environmentVariables||{}}),void 0!==o.runtimeOptions&&o.runtimeOptions!==e.runtimeOptions&&(o.runtimeOptions=[...e.runtimeOptions||[],...o.runtimeOptions||[]]),Object.assign(e,o)}function ve(e,t){if(e===t)return e;const o={...t};return o.config&&(e.config||(e.config={}),o.config=ye(e.config,o.config)),Object.assign(e,o)}function _e(e,t){if(e===t)return e;const o={...t};return void 0!==o.coreAssembly&&(o.coreAssembly=[...e.coreAssembly||[],...o.coreAssembly||[]]),void 0!==o.assembly&&(o.assembly=[...e.assembly||[],...o.assembly||[]]),void 0!==o.lazyAssembly&&(o.lazyAssembly=[...e.lazyAssembly||[],...o.lazyAssembly||[]]),void 0!==o.corePdb&&(o.corePdb=[...e.corePdb||[],...o.corePdb||[]]),void 0!==o.pdb&&(o.pdb=[...e.pdb||[],...o.pdb||[]]),void 0!==o.jsModuleWorker&&(o.jsModuleWorker=[...e.jsModuleWorker||[],...o.jsModuleWorker||[]]),void 0!==o.jsModuleNative&&(o.jsModuleNative=[...e.jsModuleNative||[],...o.jsModuleNative||[]]),void 0!==o.jsModuleDiagnostics&&(o.jsModuleDiagnostics=[...e.jsModuleDiagnostics||[],...o.jsModuleDiagnostics||[]]),void 0!==o.jsModuleRuntime&&(o.jsModuleRuntime=[...e.jsModuleRuntime||[],...o.jsModuleRuntime||[]]),void 0!==o.wasmSymbols&&(o.wasmSymbols=[...e.wasmSymbols||[],...o.wasmSymbols||[]]),void 0!==o.wasmNative&&(o.wasmNative=[...e.wasmNative||[],...o.wasmNative||[]]),void 0!==o.icu&&(o.icu=[...e.icu||[],...o.icu||[]]),void 0!==o.satelliteResources&&(o.satelliteResources=function(e,t){if(e===t)return e;for(const o in t)e[o]=[...e[o]||[],...t[o]||[]];return e}(e.satelliteResources||{},o.satelliteResources||{})),void 0!==o.modulesAfterConfigLoaded&&(o.modulesAfterConfigLoaded=[...e.modulesAfterConfigLoaded||[],...o.modulesAfterConfigLoaded||[]]),void 0!==o.modulesAfterRuntimeReady&&(o.modulesAfterRuntimeReady=[...e.modulesAfterRuntimeReady||[],...o.modulesAfterRuntimeReady||[]]),void 0!==o.extensions&&(o.extensions={...e.extensions||{},...o.extensions||{}}),void 0!==o.vfs&&(o.vfs=[...e.vfs||[],...o.vfs||[]]),Object.assign(e,o)}function Ee(){const e=Pe.config;if(e.environmentVariables=e.environmentVariables||{},e.runtimeOptions=e.runtimeOptions||[],e.resources=e.resources||{assembly:[],jsModuleNative:[],jsModuleWorker:[],jsModuleRuntime:[],wasmNative:[],vfs:[],satelliteResources:{}},e.assets){Pe.diagnosticTracing&&b("config.assets is deprecated, use config.resources instead");for(const t of e.assets){const o={};switch(t.behavior){case"assembly":o.assembly=[t];break;case"pdb":o.pdb=[t];break;case"resource":o.satelliteResources={},o.satelliteResources[t.culture]=[t];break;case"icu":o.icu=[t];break;case"symbols":o.wasmSymbols=[t];break;case"vfs":o.vfs=[t];break;case"dotnetwasm":o.wasmNative=[t];break;case"js-module-threads":o.jsModuleWorker=[t];break;case"js-module-runtime":o.jsModuleRuntime=[t];break;case"js-module-native":o.jsModuleNative=[t];break;case"js-module-diagnostics":o.jsModuleDiagnostics=[t];break;case"js-module-dotnet":break;default:throw new Error(`Unexpected behavior ${t.behavior} of asset ${t.name}`)}_e(e.resources,o)}}e.debugLevel,void 0===e.virtualWorkingDirectory&&(e.virtualWorkingDirectory=Ie),e.applicationEnvironment||(e.applicationEnvironment="Production"),e.applicationCulture&&(e.environmentVariables.LANG=`${e.applicationCulture}.UTF-8`),Oe.diagnosticTracing=Pe.diagnosticTracing=!!e.diagnosticTracing,Oe.waitForDebugger=e.waitForDebugger,Pe.maxParallelDownloads=e.maxParallelDownloads||Pe.maxParallelDownloads,Pe.enableDownloadRetry=void 0!==e.enableDownloadRetry?e.enableDownloadRetry:Pe.enableDownloadRetry}let Re=!1;async function je(e){var t;if(Re)return void await Pe.afterConfigLoaded.promise;let o;try{if(e.configSrc||Pe.config&&0!==Object.keys(Pe.config).length&&(Pe.config.assets||Pe.config.resources)||(e.configSrc="dotnet.boot.js"),o=e.configSrc,Re=!0,o&&(Pe.diagnosticTracing&&b("mono_wasm_load_config"),await async function(e){const t=e.configSrc,o=Pe.locateFile(t);let n=null;void 0!==Pe.loadBootResource&&(n=Pe.loadBootResource("manifest",t,o,"","manifest"));let i,r=null;if(n)if("string"==typeof n)n.includes(".json")?(r=await s(C(n)),i=await xe(r)):i=(await import(C(n))).config;else{const e=await n;"function"==typeof e.json?(r=e,i=await xe(r)):i=e.config}else o.includes(".json")?(r=await s(le(o,"manifest")),i=await xe(r)):i=(await import(le(o,"manifest"))).config;function s(e){return Pe.fetch_like(e,{method:"GET",credentials:"include",cache:"no-cache"})}Pe.config.applicationEnvironment&&(i.applicationEnvironment=Pe.config.applicationEnvironment),ye(Pe.config,i)}(e)),Ee(),await he(null===(t=Pe.config.resources)||void 0===t?void 0:t.modulesAfterConfigLoaded),await be("onRuntimeConfigLoaded",[Pe.config]),e.onConfigLoaded)try{await e.onConfigLoaded(Pe.config,Le),Ee()}catch(e){throw _("onConfigLoaded() failed",e),e}Ee(),Pe.afterConfigLoaded.promise_control.resolve(Pe.config)}catch(t){const n=`Failed to load config file ${o} ${t} ${null==t?void 0:t.stack}`;throw Pe.config=e.config=Object.assign(Pe.config,{message:n,error:t,isError:!0}),Xe(1,new Error(n)),t}}function Te(){return!!globalThis.navigator&&(Pe.isChromium||Pe.isFirefox)}async function xe(e){const t=Pe.config,o=await e.json();t.applicationEnvironment||o.applicationEnvironment||(o.applicationEnvironment=e.headers.get("Blazor-Environment")||e.headers.get("DotNet-Environment")||void 0),o.environmentVariables||(o.environmentVariables={});const n=e.headers.get("DOTNET-MODIFIABLE-ASSEMBLIES");n&&(o.environmentVariables.DOTNET_MODIFIABLE_ASSEMBLIES=n);const i=e.headers.get("ASPNETCORE-BROWSER-TOOLS");return i&&(o.environmentVariables.__ASPNETCORE_BROWSER_TOOLS=i),o}"function"==typeof importScripts&&(globalThis.dotnetSidecar=!0);const Ae="object"==typeof process&&"object"==typeof process.versions&&"string"==typeof process.versions.node,De="function"==typeof importScripts,Se=De&&"undefined"!=typeof dotnetSidecar,ke=De&&!Se,Me="object"==typeof window||De&&!Ae,Ce=!Me&&!Ae,Ie="/";let Oe={},Pe={},Ue={},Le={},Ne={},$e=!1;const ze={},We={config:ze},Fe={mono:{},binding:{},internal:Ne,module:We,loaderHelpers:Pe,runtimeHelpers:Oe,diagnosticHelpers:Ue,api:Le};function Be(e,t){if(e)return;const o="Assert failed: "+("function"==typeof t?t():t),n=new Error(o);_(o,n),Oe.nativeAbort(n)}function Ve(){return void 0!==Pe.exitCode}function He(){return Oe.runtimeReady&&!Ve()}function Je(){Ve()&&Be(!1,`.NET runtime already exited with ${Pe.exitCode} ${Pe.exitReason}. You can use dotnet.runMain() which doesn't exit the runtime.`),Oe.runtimeReady||Be(!1,".NET runtime didn't start yet. Please call dotnet.create() first.")}function qe(){Me&&(globalThis.addEventListener("unhandledrejection",et),globalThis.addEventListener("error",tt))}let Qe,Ge;function Ze(e){Ge&&Ge(e),Xe(e,Pe.exitReason)}function Ke(e){Qe&&Qe(e||Pe.exitReason),Xe(1,e||Pe.exitReason)}function Xe(e,t){var o;const n=t&&"object"==typeof t;e=n&&"number"==typeof t.status?t.status:void 0===e?-1:e;const i=n&&"string"==typeof t.message?t.message:""+t;(t=n?t:Oe.ExitStatus?function(e,t){const o=new Oe.ExitStatus(e);return o.message=t,o.toString=()=>t,o}(e,i):new Error("Exit with code "+e+" "+i)).status=e,t.message||(t.message=i);const r=""+(t.stack||(new Error).stack);try{Object.defineProperty(t,"stack",{get:()=>r})}catch(e){}const s=!!t.silent;if(t.silent=!0,Ve())Pe.diagnosticTracing&&b("mono_exit called after exit");else{try{We.onAbort==Ke&&(We.onAbort=Qe),We.onExit==Ze&&(We.onExit=Ge),Me&&(globalThis.removeEventListener("unhandledrejection",et),globalThis.removeEventListener("error",tt)),Oe.runtimeReady?(Oe.jiterpreter_dump_stats&&Oe.jiterpreter_dump_stats(!1),0===e&&(null===(o=Pe.config)||void 0===o?void 0:o.interopCleanupOnExit)&&Oe.forceDisposeProxies(!0,!0)):(Pe.diagnosticTracing&&b(`abort_startup, reason: ${t}`),function(e){Pe.allDownloadsQueued.promise_control.reject(e),Pe.allDownloadsFinished.promise_control.reject(e),Pe.afterConfigLoaded.promise_control.reject(e),Pe.wasmCompilePromise.promise_control.reject(e),Pe.runtimeModuleLoaded.promise_control.reject(e),Oe.dotnetReady&&(Oe.dotnetReady.promise_control.reject(e),Oe.afterInstantiateWasm.promise_control.reject(e),Oe.afterPreRun.promise_control.reject(e),Oe.beforeOnRuntimeInitialized.promise_control.reject(e),Oe.afterOnRuntimeInitialized.promise_control.reject(e),Oe.afterPostRun.promise_control.reject(e))}(t))}catch(e){v("mono_exit A failed",e)}try{s||(function(e,t){if(0!==e&&t){const e=Oe.ExitStatus&&t instanceof Oe.ExitStatus?b:_;"string"==typeof t?e(t):(void 0===t.stack&&(t.stack=(new Error).stack+""),t.message?e(Oe.stringify_as_error_with_stack?Oe.stringify_as_error_with_stack(t.message+"\n"+t.stack):t.message+"\n"+t.stack):e(JSON.stringify(t)))}!ke&&Pe.config&&(Pe.config.logExitCode?Pe.config.forwardConsole?j("WASM EXIT "+e):y("WASM EXIT "+e):Pe.config.forwardConsole&&j())}(e,t),function(e){if(Me&&!ke&&Pe.config&&Pe.config.appendElementOnExit&&document){const t=document.createElement("label");t.id="tests_done",0!==e&&(t.style.background="red"),t.innerHTML=""+e,document.body.appendChild(t)}}(e))}catch(e){v("mono_exit B failed",e)}Pe.exitCode=e,Pe.exitReason||(Pe.exitReason=t),!ke&&Oe.runtimeReady&&We.runtimeKeepalivePop()}if(Pe.config&&Pe.config.asyncFlushOnExit&&0===e)throw(async()=>{try{await async function(){try{const e=await import(/*! webpackIgnore: true */"process"),t=e=>new Promise((t,o)=>{e.on("error",o),e.end("","utf8",t)}),o=t(e.stderr),n=t(e.stdout);let i;const r=new Promise(e=>{i=setTimeout(()=>e("timeout"),1e3)});await Promise.race([Promise.all([n,o]),r]),clearTimeout(i)}catch(e){_(`flushing std* streams failed: ${e}`)}}()}finally{Ye(e,t)}})(),t;Ye(e,t)}function Ye(e,t){if(Oe.runtimeReady&&Oe.nativeExit)try{Oe.nativeExit(e)}catch(e){!Oe.ExitStatus||e instanceof Oe.ExitStatus||v("set_exit_code_and_quit_now failed: "+e.toString())}if(0!==e||!Me)throw Ae?process.exit(e):Oe.quit&&Oe.quit(e,t),t}function et(e){ot(e,e.reason,"rejection")}function tt(e){ot(e,e.error,"error")}function ot(e,t,o){e.preventDefault();try{t||(t=new Error("Unhandled "+o)),void 0===t.stack&&(t.stack=(new Error).stack),t.stack=t.stack+"",t.silent||(_("Unhandled error:",t),Xe(1,t))}catch(e){}}!function(n){if($e)throw new Error("Loader module already loaded");$e=!0,Oe=n.runtimeHelpers,Pe=n.loaderHelpers,Ue=n.diagnosticHelpers,Le=n.api,Ne=n.internal,Object.assign(Le,{INTERNAL:Ne,invokeLibraryInitializers:be}),Object.assign(n.module,{config:ye(ze,{environmentVariables:{}})});const a={mono_wasm_bindings_is_ready:!1,config:n.module.config,diagnosticTracing:!1,nativeAbort:e=>{throw e||new Error("abort")},nativeExit:e=>{throw new Error("exit:"+e)}},l={gitHash:"17d11de66cf75b962995c81dd1235fae9aa5ece0",config:n.module.config,diagnosticTracing:!1,maxParallelDownloads:16,enableDownloadRetry:!0,_loaded_files:[],loadedFiles:[],loadedAssemblies:[],libraryInitializers:[],workerNextNumber:1,actual_downloaded_assets_count:0,actual_instantiated_assets_count:0,expected_downloaded_assets_count:0,expected_instantiated_assets_count:0,afterConfigLoaded:i(),allDownloadsQueued:i(),allDownloadsFinished:i(),wasmCompilePromise:i(),runtimeModuleLoaded:i(),loadingWorkers:i(),is_exited:Ve,is_runtime_running:He,assert_runtime_running:Je,mono_exit:Xe,createPromiseController:i,getPromiseController:r,assertIsControllablePromise:s,mono_download_assets:te,resolve_single_asset_path:Y,setup_proxy_console:R,set_thread_prefix:h,installUnhandledErrorHandler:qe,retrieve_asset_download:ie,invokeLibraryInitializers:be,isDebuggingSupported:Te,exceptions:e,simd:o,relaxedSimd:t};Object.assign(Oe,a),Object.assign(Pe,l)}(Fe);let nt,it,rt,st=!1,at=!1;async function lt(e){if(!at){if(at=!0,Me&&Pe.config.forwardConsole&&void 0!==globalThis.WebSocket&&R("main",globalThis.console,globalThis.location.origin),We||Be(!1,"Null moduleConfig"),Pe.config||Be(!1,"Null moduleConfig.config"),"function"==typeof e){const t=e(Fe.api);if(t.ready)throw new Error("Module.ready couldn't be redefined.");Object.assign(We,t),ve(We,t)}else{if("object"!=typeof e)throw new Error("Can't use moduleFactory callback of createDotnetRuntime function.");ve(We,e)}await async function(e){if(Ae){const e=await import(/*! webpackIgnore: true */"process"),t=14;if(e.versions.node.split(".")[0]<t)throw new Error(`NodeJS at '${e.execPath}' has too low version '${e.versions.node}', please use at least ${t}. See also https://aka.ms/dotnet-wasm-features`)}const t=/*! webpackIgnore: true */import.meta.url,o=t.indexOf("?");var n;if(o>0&&(Pe.modulesUniqueQuery=t.substring(o)),Pe.scriptUrl=t.replace(/\\/g,"/").replace(/[?#].*/,""),Pe.scriptDirectory=(n=Pe.scriptUrl).slice(0,n.lastIndexOf("/"))+"/",Pe.locateFile=e=>"URL"in globalThis&&globalThis.URL!==k?new URL(e,Pe.scriptDirectory).toString():P(e)?e:Pe.scriptDirectory+e,Pe.fetch_like=M,Pe.out=console.log,Pe.err=console.error,Pe.onDownloadResourceProgress=e.onDownloadResourceProgress,Me&&globalThis.navigator){const e=globalThis.navigator,t=e.userAgentData&&e.userAgentData.brands;t&&t.length>0?Pe.isChromium=t.some(e=>"Google Chrome"===e.brand||"Microsoft Edge"===e.brand||"Chromium"===e.brand):e.userAgent&&(Pe.isChromium=e.userAgent.includes("Chrome"),Pe.isFirefox=e.userAgent.includes("Firefox"))}void 0===globalThis.URL&&(globalThis.URL=k)}(We)}}async function ct(e){return await lt(e),Pe.config.exitOnUnhandledError&&qe(),Qe=We.onAbort,Ge=We.onExit,We.onAbort=Ke,We.onExit=Ze,We.ENVIRONMENT_IS_PTHREAD?async function(){(function(){const e=new MessageChannel,t=e.port1,o=e.port2;t.addEventListener("message",e=>{var n,i;n=JSON.parse(e.data.config),i=JSON.parse(e.data.monoThreadInfo),st?Pe.diagnosticTracing&&b("mono config already received"):(ye(Pe.config,n),Oe.monoThreadInfo=i,Ee(),Pe.diagnosticTracing&&b("mono config received"),st=!0,Pe.afterConfigLoaded.promise_control.resolve(Pe.config),Me&&n.forwardConsole&&void 0!==globalThis.WebSocket&&Pe.setup_proxy_console("worker-idle",console,globalThis.location.origin)),t.close(),o.close()},{once:!0}),t.start(),self.postMessage({[a]:{monoCmd:"preload",port:o}},[o])})(),await Pe.afterConfigLoaded.promise,function(){const e=Pe.config;e.assets||Be(!1,"config.assets must be defined");for(const t of e.assets)K(t),Q[t.behavior]&&$.push(t)}(),setTimeout(async()=>{try{await te()}catch(e){Xe(1,e)}},0);const e=dt(),t=await Promise.all(e);return await ut(t),We}():async function(){var e;await je(We),ne();const t=dt();(async function(){try{const e=Y("dotnetwasm");await re(e),e&&e.pendingDownloadInternal&&e.pendingDownloadInternal.response||Be(!1,"Can't load dotnet.native.wasm");const t=await e.pendingDownloadInternal.response,o=t.headers&&t.headers.get?t.headers.get("Content-Type"):void 0;let n;if("function"==typeof WebAssembly.compileStreaming&&"application/wasm"===o)n=await WebAssembly.compileStreaming(t);else{Me&&"application/wasm"!==o&&v('WebAssembly resource does not have the expected content type "application/wasm", so falling back to slower ArrayBuffer instantiation.');const e=await t.arrayBuffer();Pe.diagnosticTracing&&b("instantiate_wasm_module buffered"),n=Ce?await Promise.resolve(new WebAssembly.Module(e)):await WebAssembly.compile(e)}e.pendingDownloadInternal=null,e.pendingDownload=null,e.buffer=null,e.moduleExports=null,Pe.wasmCompilePromise.promise_control.resolve(n)}catch(e){Pe.wasmCompilePromise.promise_control.reject(e)}})(),setTimeout(async()=>{try{D(),await te()}catch(e){Xe(1,e)}},0);const o=await Promise.all(t);return await ut(o),await Oe.dotnetReady.promise,await he(null===(e=Pe.config.resources)||void 0===e?void 0:e.modulesAfterRuntimeReady),await be("onRuntimeReady",[Fe.api]),Le}()}function dt(){const e=Y("js-module-runtime"),t=Y("js-module-native");if(nt&&it)return[nt,it,rt];"object"==typeof e.moduleExports?nt=e.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${e.resolvedUrl}' for ${e.name}`),nt=import(/*! webpackIgnore: true */e.resolvedUrl)),"object"==typeof t.moduleExports?it=t.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${t.resolvedUrl}' for ${t.name}`),it=import(/*! webpackIgnore: true */t.resolvedUrl));const o=X("js-module-diagnostics");return o&&("object"==typeof o.moduleExports?rt=o.moduleExports:(Pe.diagnosticTracing&&b(`Attempting to import '${o.resolvedUrl}' for ${o.name}`),rt=import(/*! webpackIgnore: true */o.resolvedUrl))),[nt,it,rt]}async function ut(e){const{initializeExports:t,initializeReplacements:o,configureRuntimeStartup:n,configureEmscriptenStartup:i,configureWorkerStartup:r,setRuntimeGlobals:s,passEmscriptenInternals:a}=e[0],{default:l}=e[1],c=e[2];s(Fe),t(Fe),c&&c.setRuntimeGlobals(Fe),await n(We),Pe.runtimeModuleLoaded.promise_control.resolve(),l(()=>(Object.assign(We,{__dotnet_runtime:{initializeReplacements:o,configureEmscriptenStartup:i,configureWorkerStartup:r,passEmscriptenInternals:a}}),We)).catch(e=>{if(e.message&&e.message.toLowerCase().includes("out of memory"))throw new Error(".NET runtime has failed to start, because too much memory was requested. Please decrease the memory by adjusting EmccMaximumHeapSize. See also https://aka.ms/dotnet-wasm-features");throw e})}const ft=new class{withModuleConfig(e){try{return ve(We,e),this}catch(e){throw Xe(1,e),e}}withInterpreterPgo(e,t){try{return ye(ze,{interpreterPgo:e,interpreterPgoSaveDelay:t}),ze.runtimeOptions?ze.runtimeOptions.push("--interp-pgo-recording"):ze.runtimeOptions=["--interp-pgo-recording"],this}catch(e){throw Xe(1,e),e}}withConfig(e){try{return ye(ze,e),this}catch(e){throw Xe(1,e),e}}withConfigSrc(e){try{return e&&"string"==typeof e||Be(!1,"must be file path or URL"),ve(We,{configSrc:e}),this}catch(e){throw Xe(1,e),e}}withVirtualWorkingDirectory(e){try{return e&&"string"==typeof e||Be(!1,"must be directory path"),ye(ze,{virtualWorkingDirectory:e}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariable(e,t){try{const o={};return o[e]=t,ye(ze,{environmentVariables:o}),this}catch(e){throw Xe(1,e),e}}withEnvironmentVariables(e){try{return e&&"object"==typeof e||Be(!1,"must be dictionary object"),ye(ze,{environmentVariables:e}),this}catch(e){throw Xe(1,e),e}}withDiagnosticTracing(e){try{return"boolean"!=typeof e&&Be(!1,"must be boolean"),ye(ze,{diagnosticTracing:e}),this}catch(e){throw Xe(1,e),e}}withDebugging(e){try{return null!=e&&"number"==typeof e||Be(!1,"must be number"),ye(ze,{debugLevel:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArguments(...e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ye(ze,{applicationArguments:e}),this}catch(e){throw Xe(1,e),e}}withRuntimeOptions(e){try{return e&&Array.isArray(e)||Be(!1,"must be array of strings"),ze.runtimeOptions?ze.runtimeOptions.push(...e):ze.runtimeOptions=e,this}catch(e){throw Xe(1,e),e}}withMainAssembly(e){try{return ye(ze,{mainAssemblyName:e}),this}catch(e){throw Xe(1,e),e}}withApplicationArgumentsFromQuery(){try{if(!globalThis.window)throw new Error("Missing window to the query parameters from");if(void 0===globalThis.URLSearchParams)throw new Error("URLSearchParams is supported");const e=new URLSearchParams(globalThis.window.location.search).getAll("arg");return this.withApplicationArguments(...e)}catch(e){throw Xe(1,e),e}}withApplicationEnvironment(e){try{return ye(ze,{applicationEnvironment:e}),this}catch(e){throw Xe(1,e),e}}withApplicationCulture(e){try{return ye(ze,{applicationCulture:e}),this}catch(e){throw Xe(1,e),e}}withResourceLoader(e){try{return Pe.loadBootResource=e,this}catch(e){throw Xe(1,e),e}}async download(){try{await async function(){lt(We),await je(We),ne(),D(),te(),await Pe.allDownloadsFinished.promise}()}catch(e){throw Xe(1,e),e}}async create(){try{return this.instance||(this.instance=await async function(){return await ct(We),Fe.api}()),this.instance}catch(e){throw Xe(1,e),e}}run(){return this.runMainAndExit()}async runMainAndExit(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMainAndExit()}catch(e){throw Xe(1,e),e}}async runMain(){try{return We.config||Be(!1,"Null moduleConfig.config"),this.instance||await this.create(),this.instance.runMain()}catch(e){throw Xe(1,e),e}}},mt=Xe,gt=ct;Ce||"function"==typeof globalThis.URL||Be(!1,"This browser/engine doesn't support URL API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),"function"!=typeof globalThis.BigInt64Array&&Be(!1,"This browser/engine doesn't support BigInt64Array API. Please use a modern version. See also https://aka.ms/dotnet-wasm-features"),globalThis.performance&&"function"==typeof globalThis.performance.now||Be(!1,"This browser/engine doesn't support performance.now. Please use a modern version."),Ce||globalThis.crypto&&"object"==typeof globalThis.crypto.subtle||Be(!1,"This engine doesn't support crypto.subtle. Please use a modern version."),Ce||globalThis.crypto&&"function"==typeof globalThis.crypto.getRandomValues||Be(!1,"This engine doesn't support crypto.getRandomValues. Please use a modern version."),Ae&&"function"!=typeof process.exit&&Be(!1,"This engine doesn't support process.exit. Please use a modern version."),ft.withConfig(/*json-start*/{
  "mainAssemblyName": "AwesomeBlazorBrowser",
  "resources": {
    "hash": "sha256-vsYuoeN2dSUvZHpkaZvtWVSpOWCk/F3HJS+nrmCMkLw=",
    "jsModuleNative": [
      {
        "name": "dotnet.native.h9vwbryp09.js"
      }
    ],
    "jsModuleRuntime": [
      {
        "name": "dotnet.runtime.6ti4e6r1z8.js"
      }
    ],
    "wasmNative": [
      {
        "name": "dotnet.native.xixoi8exul.wasm",
        "hash": "sha256-R67gfXAk40qYZQKLU1QMmUV8TPrzf0n9tnY1V8YKlRA=",
        "cache": "force-cache"
      }
    ],
    "coreAssembly": [
      {
        "virtualPath": "System.Private.CoreLib.wasm",
        "name": "System.Private.CoreLib.y0d092n0r0.wasm",
        "hash": "sha256-8qLhDF9ohMBPmRaNCOVPKL/rRJZ/uhJORsP0HHxGvjc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.JavaScript.wasm",
        "name": "System.Runtime.InteropServices.JavaScript.mg7wfpld9e.wasm",
        "hash": "sha256-xvDHjBUByp0603tDRRJJtkdFMlxAznTN0RLC8JSJxtI=",
        "cache": "force-cache"
      }
    ],
    "assembly": [
      {
        "virtualPath": "AwesomeBlazor.Models.wasm",
        "name": "AwesomeBlazor.Models.2pxys7lnq9.wasm",
        "hash": "sha256-6vgc4Y19WnjMfmSYN0uonm1BH4oSxto1FlMinvQ6jFE=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "AwesomeBlazorBrowser.wasm",
        "name": "AwesomeBlazorBrowser.dytqdb8pty.wasm",
        "hash": "sha256-2+xYvKdV5Xmb8bEWvYz9i8mde/Vi8SAdm9lH4x4AJ5o=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Markdig.wasm",
        "name": "Markdig.utvtmyk454.wasm",
        "hash": "sha256-ntsheG6v4c/CZrLHmK247bkKLf2Zj/VDMtQyTI0E8d8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.wasm",
        "name": "Microsoft.AspNetCore.Components.63aef60atn.wasm",
        "hash": "sha256-kvFhosmoVVlN15VjHxmGUvoNfclWRG28OwQsjmrJ1e4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.Web.wasm",
        "name": "Microsoft.AspNetCore.Components.Web.lk39t29tq4.wasm",
        "hash": "sha256-qzAeY9etbrCbNVgMe2BhzWC7vpO+T6ZRQxXI+oqtvY0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.AspNetCore.Components.WebAssembly.wasm",
        "name": "Microsoft.AspNetCore.Components.WebAssembly.b3q471n9w5.wasm",
        "hash": "sha256-6ee/4KCzvgDxHWNecQPbKygoR7pBobfeuXLxgFGoYfc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.wasm",
        "name": "Microsoft.Extensions.Configuration.phib19uyzk.wasm",
        "hash": "sha256-owhTXlHXka/aXZ5tP/7tkYyCCAVYN6xsg3NuMoaXj0A=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Abstractions.wasm",
        "name": "Microsoft.Extensions.Configuration.Abstractions.aayihhhxhu.wasm",
        "hash": "sha256-7pxarWpIyt6+ZOA+0GagvP4VJa3lwqxSVNSSxLBu4LI=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.EnvironmentVariables.wasm",
        "name": "Microsoft.Extensions.Configuration.EnvironmentVariables.9mqtai7c9y.wasm",
        "hash": "sha256-/5FFNZEBWSjUUR5XaPsOxTAyg1S4IbJ3YXaSqxl+FH8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Configuration.Json.wasm",
        "name": "Microsoft.Extensions.Configuration.Json.5oy18zbh6o.wasm",
        "hash": "sha256-xclwtOFbV0cjDARrm/E6EaaFnzkO0pGiwodRBpc2bow=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.Abstractions.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.Abstractions.yhn526arji.wasm",
        "hash": "sha256-yn/GF9gaZrK8JNgUX2X/LDW2HMA8OtU9BUHhXG7UgXk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.DependencyInjection.wasm",
        "name": "Microsoft.Extensions.DependencyInjection.4z37har4tz.wasm",
        "hash": "sha256-4NhrO5U0ADBkPAEga7th6lgUay2cSCetSv3kY1z/3EM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.wasm",
        "name": "Microsoft.Extensions.Diagnostics.pzqx2nvtb9.wasm",
        "hash": "sha256-G8sgiWHzLuSOligCIN4D34kgukaR0Mt/FfAqY3M++wU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Diagnostics.Abstractions.wasm",
        "name": "Microsoft.Extensions.Diagnostics.Abstractions.w7yw8eqnqo.wasm",
        "hash": "sha256-PNViiZsQQgNthmBDEg7Swt1vjx9k95whnubxDTKS7pg=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Hosting.Abstractions.wasm",
        "name": "Microsoft.Extensions.Hosting.Abstractions.t9p127yvf2.wasm",
        "hash": "sha256-MdSfdGTWCBZhXtP0GqvxVMVsUyL4tL1hJd2BePktvec=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.Abstractions.wasm",
        "name": "Microsoft.Extensions.Logging.Abstractions.0jk1cthv74.wasm",
        "hash": "sha256-1PGlxeyHAl/v2nlhZzAY92vOqyNsJvt9kMxg9PGydQ0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Logging.wasm",
        "name": "Microsoft.Extensions.Logging.nrvqtt7o00.wasm",
        "hash": "sha256-rZvRDrlOGFOnjkH+PJ7qph3Fyi2H69f8cugbqTQrLRw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Options.wasm",
        "name": "Microsoft.Extensions.Options.rogdycdygb.wasm",
        "hash": "sha256-u/fEvfN+oe969txHrIy9Lm/LRga+zzy5+tINAgxrxKQ=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.Extensions.Primitives.wasm",
        "name": "Microsoft.Extensions.Primitives.u6yaseehjv.wasm",
        "hash": "sha256-ln9OeKoV6WNPE9d2IR+8kYPJCyFBaXeKHzQaLHd9CZU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.WebAssembly.wasm",
        "name": "Microsoft.JSInterop.WebAssembly.tzst4j5q7q.wasm",
        "hash": "sha256-90edrw1wyRDqfOBnTp6MEddxOD8cIgKgqrOxLxZAA9k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Microsoft.JSInterop.wasm",
        "name": "Microsoft.JSInterop.tx1ildt7wk.wasm",
        "hash": "sha256-rwxWJdWIIpWNk5Buqdxk6krUlhnf8B8sD2xsKe6EXek=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Concurrent.wasm",
        "name": "System.Collections.Concurrent.e54ygt1eki.wasm",
        "hash": "sha256-gn0Jz1ICS0VcGbqptUiSCFmbc4PSSPtDJ5cRS9ktZ8E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Immutable.wasm",
        "name": "System.Collections.Immutable.v4c8dy5sjk.wasm",
        "hash": "sha256-bzR6NKLvrfwRQPnBUEx4o2cHyXkqawyBohVXXo8ZG88=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.Specialized.wasm",
        "name": "System.Collections.Specialized.i2dx130v8s.wasm",
        "hash": "sha256-ag3TF3j4pHUVObz3Wvot55Jvzu60o1+aASN7394WSgk=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Collections.wasm",
        "name": "System.Collections.v6rt8h6tlb.wasm",
        "hash": "sha256-GUVw6idlvRuPyuyZHkTQJIOZuHPgPFOgkp0ccCN3ECA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.Primitives.wasm",
        "name": "System.ComponentModel.Primitives.fn1onz7co4.wasm",
        "hash": "sha256-W3HfjeYfbVlH5DNfoXNOWEMOlXF0+SdiWvEW34zfb+k=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.TypeConverter.wasm",
        "name": "System.ComponentModel.TypeConverter.bnl6k5okpf.wasm",
        "hash": "sha256-F/vFR6ceDVAZe65Qjy4xqZcA6dP5GEi90g8um8975lo=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ComponentModel.wasm",
        "name": "System.ComponentModel.wvxriojdvg.wasm",
        "hash": "sha256-TAbnqhqH8PkbMkluwc+0uWncVd1npnwgUe+GMqdTr3w=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Console.wasm",
        "name": "System.Console.7hsm3l3dic.wasm",
        "hash": "sha256-qwAVhKQsT+sZK+/Gp7yakCgSP+jdXIKOBuBsESqcj+E=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Diagnostics.DiagnosticSource.wasm",
        "name": "System.Diagnostics.DiagnosticSource.ehu03u5t04.wasm",
        "hash": "sha256-rrrqv/r41hWGimjVUx2fLX9CQaep45yWFXmxrYRzkBw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.IO.Pipelines.wasm",
        "name": "System.IO.Pipelines.fhb6t19vdo.wasm",
        "hash": "sha256-XU8LNiQSXYXhCTIaeqLiXc1gd9G451M5+dYxwc5pDs4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Linq.wasm",
        "name": "System.Linq.1xxefbpk0r.wasm",
        "hash": "sha256-y1E+oEI85e6PGwOtZl6xi13rR8iGwyUwK8L4wV3zJJs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Memory.wasm",
        "name": "System.Memory.benhyk3rs3.wasm",
        "hash": "sha256-s7LO5agRw0Un99FohAvkE6mBassL4PFTxuzCUyrGdSs=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Http.wasm",
        "name": "System.Net.Http.9cqpzkcevn.wasm",
        "hash": "sha256-mWRvdAWhoMoDkYcAaNFWTgMUcOmqGNDi30gZu/jE5FM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Net.Primitives.wasm",
        "name": "System.Net.Primitives.0dcdqtachx.wasm",
        "hash": "sha256-LPLw20hRlxuD7lrWEXTMMJxIKZcevEEOvS7pa/Er+xU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.ObjectModel.wasm",
        "name": "System.ObjectModel.fkxz9cmlf6.wasm",
        "hash": "sha256-UXIivuV4WJWvLjmleB0VXL/w3a6TX8CPoXsio9CxCSc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Private.Uri.wasm",
        "name": "System.Private.Uri.aq9rnx7qxh.wasm",
        "hash": "sha256-AvUA5jrZNFQcf/8kUj9ZJlJJfhknAZdxq96l1EV+lRM=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.InteropServices.wasm",
        "name": "System.Runtime.InteropServices.13rmclkh2l.wasm",
        "hash": "sha256-Y03XFlpsbyPpntn4L2CD/ay4P30B7hRM4wZQ6w8xuBA=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Runtime.wasm",
        "name": "System.Runtime.c1b87270kq.wasm",
        "hash": "sha256-IPeLOdusOKvMOHIRWzPKZLfk5VlIcwohIIaF4DUP2qY=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Security.Cryptography.wasm",
        "name": "System.Security.Cryptography.k1fe8qw45w.wasm",
        "hash": "sha256-qPPbwx9sYJoYWq6ah+8q4eg4/u6LVmQMqampmBjbocw=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Encodings.Web.wasm",
        "name": "System.Text.Encodings.Web.tfg1kqx5tl.wasm",
        "hash": "sha256-h2pwCymvwjevPmqSbPu7h+OqaOUFS8KBtsLolKvQrqU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.Json.wasm",
        "name": "System.Text.Json.xo8wslrmoa.wasm",
        "hash": "sha256-ATTEZdQoY5gXKC2tm/5ULWkDN8+RxXBKcvjmUojVFTc=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Text.RegularExpressions.wasm",
        "name": "System.Text.RegularExpressions.odyvnx6ryj.wasm",
        "hash": "sha256-NIWTDJW3jTCT8Vq19bcJxtINK+neGdr4l7X6im/S1a0=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Threading.wasm",
        "name": "System.Threading.uafib0usxb.wasm",
        "hash": "sha256-9ui2dDAuGUdswZy7oDiw9ePafJI++NCRP+uL2RvIXs4=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.Web.HttpUtility.wasm",
        "name": "System.Web.HttpUtility.now24nlhgl.wasm",
        "hash": "sha256-9+Hf00Ycjno53jktqx2nj8dA7zqNHq+9LQeEP6ljin8=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "System.wasm",
        "name": "System.9rz1s012yg.wasm",
        "hash": "sha256-RZd87kH8Ql0lfFLQOSmiVy+3hNZMc4n0h0EUzUBBjmU=",
        "cache": "force-cache"
      },
      {
        "virtualPath": "Toolbelt.Web.CssClassInlineBuilder.wasm",
        "name": "Toolbelt.Web.CssClassInlineBuilder.ul34l24fqb.wasm",
        "hash": "sha256-vpelzNPgs43hZMzj91e3XXSdniqT7CxqhEnHvRTVHaw=",
        "cache": "force-cache"
      }
    ],
    "libraryInitializers": [
      {
        "name": "BlazorWasmPreRendering.Build.lfyg69o9wu.lib.module.js"
      }
    ],
    "modulesAfterConfigLoaded": [
      {
        "name": "../BlazorWasmPreRendering.Build.lfyg69o9wu.lib.module.js"
      }
    ]
  },
  "debugLevel": 0,
  "globalizationMode": "invariant",
  "extensions": {
    "blazor": {}
  },
  "runtimeConfig": {
    "runtimeOptions": {
      "configProperties": {
        "Microsoft.AspNetCore.Components.Routing.RegexConstraintSupport": false,
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
        "System.Net.Http.WasmEnableStreamingResponse": true,
        "System.Net.SocketsHttpHandler.Http3Support": false,
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
        "Microsoft.AspNetCore.Components.Endpoints.NavigationManager.DisableThrowNavigationException": false,
        "System.Diagnostics.StackTrace.IsLineNumberSupported": false
      }
    }
  }
}/*json-end*/);export{gt as default,ft as dotnet,mt as exit};
