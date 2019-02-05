function copyToClipboard() {
    // Create a "hidden" input
    var aux = document.createElement("input");
    var quoteID = getQuoteID();
    var quantity = getQuantity();
    var discountPercent = getDiscount();
    var urlString = getURLString();

    urlString += "quoteid=" + quoteID + "&";
    urlString += "quantity=" + quantity + "&";
    urlString += "discountpercent=" + discountPercent;

    // Assign it the value of the specified element
    aux.setAttribute("value", urlString);

    // Append it to the body
    document.body.appendChild(aux);

    // Highlight its content
    aux.select();

    // Copy the highlighted text
    document.execCommand("copy");

    // Remove it from the body
    document.body.removeChild(aux);
}

function hideCopyURL() {
    var btnCopy = document.getElementById('btnCopyURL');
    if (btnCopy !== null) {
        document.getElementById('btnCopyURL').style.visibility = "hidden";
    }
}

function keyPress(event, char) {
    if (char === 13) {
        var id = event.currentTarget.id;
        if (id.indexOf("txtQuoteID") !== -1 ||
            id.indexOf("txtQuantity") !== -1 ||
            id.indexOf("txtDiscount") !== -1) {
            document.getElementById("btnSubmit").click();
        } else if (id.indexOf("AccountingCost") !== -1 || id.indexOf("ComponentPrice") !== -1) {
            document.getElementById("btnRecalculate").click();
        }
    }
}