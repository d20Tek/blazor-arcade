window.addKeyListener = (dotNetRef) => {
    document.addEventListener("keydown", (event) => {
        dotNetRef.invokeMethodAsync("HandleKeydown", event.key);
    });
};

window.getGameContainerWidth = (cssSelector) => {
    let element = document.querySelector(cssSelector);
    return element ? element.clientWidth : 0;
};

window.gameResizeHandler = {
    init: (dotnetHelper) => {
        const resizeListener = () => {
            let width = document.querySelector(".game-container")?.clientWidth || 0;
            dotnetHelper.invokeMethodAsync("UpdateGameWidth", width);
        };

        window.addEventListener("resize", resizeListener);

        // Store reference so we can remove it later
        window.gameResizeHandler._resizeListener = resizeListener;
    },

    dispose: () => {
        if (window.gameResizeHandler._resizeListener) {
            window.removeEventListener("resize", window.gameResizeHandler._resizeListener);
            window.gameResizeHandler._resizeListener = null;
        }
    }
};
