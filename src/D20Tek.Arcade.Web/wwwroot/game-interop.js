window.addKeyListener = (dotNetRef) => {
    document.addEventListener("keydown", (event) => {
        dotNetRef.invokeMethodAsync("HandleKeydown", event.key);
    });
};

window.getGameContainerWidth = (cssSelector) => {
    let element = document.querySelector(cssSelector);
    return element ? element.clientWidth : 0;
};
