export function scrollToAnchor(anchorName: string, smooth?: boolean, changeUrl?: boolean): void {
    const element = document.querySelector(`a[name=${anchorName}]`);
    if (element !== null) {
        element.scrollIntoView({ behavior: smooth === true ? 'smooth' : 'auto' });
        if (changeUrl === true) {
            const href = location.href.split('#')[0];
            history.pushState(null, document.title, `${href}#${anchorName}`);
        }
    }
}

function locationHashChanged() {
    const hash = location.hash.split('#').pop() || '';
    if (hash !== '') {
        scrollToAnchor(hash, false, false);
    }
}

window.onhashchange = locationHashChanged;
