import { getTokenizedElement } from "/_content/BlazorSchool.Components.Web/BlazorTokenize.min.js"

export function capturePdf(token, blazorButtonInstance)
{
    let capturingElement = getTokenizedElement(token);
    let childElements = capturingElement.getElementsByTagName("*");

    for (var i = -1, l = childElements.length; ++i < l;)
    {
        let styles = window.getComputedStyle(childElements[i]);
        Array.from(styles).forEach(key => childElements[i].style.setProperty(key, styles.getPropertyValue(key), 'important'))
    }

    let printWindow = window.open("", "_blank", "popup=true");

    if (printWindow == undefined)
    {
        blazorButtonInstance.invokeMethodAsync("RaisePopupBlockedError");
        return;
    }

    printWindow.document.write('<html><head><title>Save to PDF</title>');
    printWindow.document.write('</head><body>');
    Array.from(childElements).forEach(element => printWindow.document.write(element.outerHTML));
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.focus();
    printWindow.print();
    printWindow.close();
}