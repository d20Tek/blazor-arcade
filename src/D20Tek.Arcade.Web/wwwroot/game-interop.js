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
        window.addEventListener("resize", () => {
            let width = document.querySelector(".game-container")?.clientWidth;
            dotnetHelper.invokeMethodAsync("UpdateGameWidth", width);
        });
    }
};