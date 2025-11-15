export const scrollToAnchor = (anchorName, smooth, changeUrl) => {
    try {
        const element = document.querySelector(`a[name=${anchorName}]`);
        if (element !== null) {
            element.scrollIntoView({ behavior: smooth === true ? 'smooth' : 'auto' });
            if (changeUrl === true) {
                const href = location.href.split('#')[0];
                history.pushState(null, document.title, `${href}#${anchorName}`);
            }
        }
    }
    catch (error) {
        console.error(error);
    }
};
export const getCurrentTheme = () => {
    return localStorage.getItem("theme") || "theme-system-default";
};
export const setCurrentTheme = (theme) => {
    localStorage.setItem("theme", theme);
    document.body.classList.remove("theme-system-default", "theme-light-mode", "theme-dark-mode");
    document.body.classList.add(theme);
};
export const installHashWatcher = () => {
    const locationHashChanged = () => {
        const hash = location.hash.split('#').pop() || '';
        if (hash !== '') {
            scrollToAnchor(hash, false, false);
        }
    };
    window.addEventListener("hashchange", locationHashChanged);
};
