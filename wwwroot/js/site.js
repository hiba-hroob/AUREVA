
(() => {
    "use strict";

    if (!window.AurevaDemo) {
        console.error("AUREVA demo store was not loaded.");
        return;
    }

    let navigating = false;

    function isInternalLink(link) {
        if (!link || !link.href) return false;
        if (link.target && link.target !== "_self") return false;
        if (link.hasAttribute("download")) return false;

        const url = new URL(link.href, window.location.href);

        return url.origin === window.location.origin &&
            !url.pathname.startsWith("/Identity/") &&
            !link.hasAttribute("data-no-spa");
    }

    async function navigate(url, addHistory = true) {
        if (navigating) return;

        const target = new URL(url, window.location.href);

        if (target.origin !== window.location.origin) {
            window.location.href = target.href;
            return;
        }

        if (
            target.pathname === window.location.pathname &&
            target.search === window.location.search
        ) {
            return;
        }

        navigating = true;
        document.documentElement.classList.add("aureva-navigating");

        try {
            const response = await fetch(target.href, {
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });

            if (!response.ok) {
                throw new Error(`Navigation failed: ${response.status}`);
            }

            const html = await response.text();
            const parsed = new DOMParser().parseFromString(html, "text/html");
            const newMain = parsed.querySelector("main");
            const currentMain = document.querySelector("main");

            if (!newMain || !currentMain) {
                throw new Error("Could not find the page content.");
            }

            currentMain.replaceChildren(
                ...Array.from(newMain.childNodes).map(node =>
                    document.importNode(node, true)
                )
            );

            document.title = parsed.title || document.title;

            if (addHistory) {
                history.pushState({}, "", target.href);
            }

            window.scrollTo(0, 0);
          



            document.dispatchEvent(new CustomEvent("aureva:page-loaded", {
                detail: { url: target.href }
            }));

            if (window.AurevaDemo &&
                target.pathname.toLowerCase().includes("/services")) {
                document.dispatchEvent(new CustomEvent("aureva:services-refresh"));
            }

        } catch (error) {
            console.error("AUREVA navigation error:", error);
            window.location.href = target.href;
        } finally {
            navigating = false;
            document.documentElement.classList.remove("aureva-navigating");
        }
    }

    document.addEventListener("click", event => {
        if (event.defaultPrevented ||
            event.button !== 0 ||
            event.metaKey ||
            event.ctrlKey ||
            event.shiftKey ||
            event.altKey) {
            return;
        }

        const link = event.target.closest("a");

        if (!isInternalLink(link)) return;

        const url = new URL(link.href, window.location.href);

        if (url.hash && url.pathname === window.location.pathname) return;

        event.preventDefault();
        navigate(url.href);
    });

    window.addEventListener("popstate", () => {
        navigate(window.location.href, false);
    });

    window.AurevaNavigation = Object.freeze({
        navigate,
        isInternalLink,
        get isNavigating() {
            return navigating;
        }
    });
})();