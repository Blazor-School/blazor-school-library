import { getTokenizedElement } from "/_content/BlazorSchool.Components.Web/BlazorTokenize.min.js"

let previousClientX = 0, previousClientY = 0;
let windowToken;

export function registerWindowTitleEvent(inputTitleToken, inputWindowToken)
{
    let element = getTokenizedElement(inputTitleToken);
    windowTitleToken = inputTitleToken;
    windowToken = inputWindowToken;
    element.onmousedown = dragMouseDown;
}

function dragMouseDown(e)
{
    e = e || window.event;
    e.preventDefault();
    previousClientX = e.clientX;
    previousClientY = e.clientY;
    document.onmouseup = unregisterDragEvent;
    document.onmousemove = handleElementDrag;
}

function handleElementDrag(e)
{
    e = e || window.event;
    e.preventDefault();
    let diffClientX = previousClientX - e.clientX;
    let diffClientY = previousClientY - e.clientY;
    previousClientX = e.clientX;
    previousClientY = e.clientY;
    let element = getTokenizedElement(windowToken);
    element.style.left = `${element.offsetLeft - diffClientX}px`;
    element.style.top = `${element.offsetTop - diffClientY}px`;
}

function unregisterDragEvent()
{
    document.onmouseup = null;
    document.onmousemove = null;
}