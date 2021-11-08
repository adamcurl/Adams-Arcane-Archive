window.shiftFocus = {
    focusElement: function (id) {
        var element = document.getElementById(id);
        if (element != null) {
            console.log("focusing");
            setTimeout(() => { element.focus() }, 100);
        }
        else {
            console.log("Not found");
        }
    }
}