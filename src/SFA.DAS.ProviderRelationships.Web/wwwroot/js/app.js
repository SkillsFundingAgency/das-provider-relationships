function AutoComplete(selectField) {
    this.selectElement = selectField
    this.selectedStandardIdSelectId = this.selectElement.id || null
}

AutoComplete.prototype.init = function() {
    this.autoComplete()
}

AutoComplete.prototype.autoComplete = function() {
    var that = this
    accessibleAutocomplete.enhanceSelectElement({
        selectElement: that.selectElement,
        minLength: 2,
        autoselect: false,
        defaultValue: '',
        displayMenu: 'overlay',
        placeholder: '',
        showAllValues: false,
        onConfirm: function (opt) {
            var txtInput = document.querySelector('#' + that.selectedStandardIdSelectId);
            var searchString = opt || txtInput.value;
            var requestedOption = [].filter.call(this.selectElement.options,
                function (option) {
                return (option.textContent || option.innerText) === searchString
                }
            )[0];
            if (requestedOption) {
                requestedOption.selected = true;
            } else {
                this.selectElement.selectedIndex = 0;
            }
        }
    });
}

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
      return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
      callback.call(window, nodes[i], i, nodes);
    }
  }

var autoCompletes = document.querySelectorAll('[data-module="autoComplete"]')

nodeListForEach(autoCompletes, function (autoComplete) {
  new AutoComplete(autoComplete).init()
})

