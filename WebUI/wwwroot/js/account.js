
$(document).ready(function () { 
    validateEmail();
}); 
 

function validateEmail() {
    $('#emailInput').on('blur', function () {
        var email = $(this).val();

        if (!email.endsWith('.com')) {
            // Display a validation message
            $('#submitButton').prop('disabled', true);
            $(this).addClass('is-invalid');
            $(this).siblings('.text-danger').text('Email must end with ".com"');
        } else {
            // Remove any previous validation messages
            $('#submitButton').prop('disabled', false);
            $(this).removeClass('is-invalid');
            $(this).siblings('.text-danger').text('');
        }
    });
};
