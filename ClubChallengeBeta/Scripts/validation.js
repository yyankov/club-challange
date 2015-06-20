jQuery.validator.unobtrusive.adapters.add(
    'notequalto', ['other'], function (options) {
        options.rules['notEqualTo'] = '#' + options.params.other;
        if (options.message) {
            options.messages['notEqualTo'] = options.message;
        }
    });

jQuery.validator.addMethod('notEqualTo', function (value, element, param) {
    return this.optional(element) || value != $(param).val();
}, '');

jQuery.validator.unobtrusive.adapters.add(
    'notequaltoto', ['other'], function (options) {
        options.rules['notEqualToTo'] = '#' + options.params.other;
        if (options.message) {
            options.messages['notEqualToTo'] = options.message;
        }
    });

jQuery.validator.addMethod('notEqualToTo', function (value, element, param) {
    return this.optional(element) || value != $(param).val();
}, '');
