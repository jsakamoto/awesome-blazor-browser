var AwesomeBlazorBrowser;
(function (AwesomeBlazorBrowser) {
    function scrollToAnchor(anchorName, smooth, changeUrl) {
        var element = document.querySelector("a[name=" + anchorName + "]");
        if (element !== null) {
            element.scrollIntoView({ behavior: smooth === true ? 'smooth' : 'auto' });
            if (changeUrl === true) {
                var href = location.href.split('#')[0];
                history.pushState(null, document.title, href + "#" + anchorName);
            }
        }
    }
    AwesomeBlazorBrowser.scrollToAnchor = scrollToAnchor;
    function locationHashChanged() {
        var hash = location.hash.split('#').pop() || '';
        if (hash !== '') {
            scrollToAnchor(hash, false, false);
        }
    }
    window.onhashchange = locationHashChanged;
})(AwesomeBlazorBrowser || (AwesomeBlazorBrowser = {}));
//# sourceMappingURL=helper.js.map