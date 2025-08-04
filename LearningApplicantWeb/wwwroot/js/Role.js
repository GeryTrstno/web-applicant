<script>
    $(document).ready(function() {
        $('#addForm').on('submit', function(e) {
            e.preventDefault(); // Prevent default form submission

            var formData = {
                Id: $('input[name="Id"]').val(),
                Name: $('input[name="Name"]').val()
            };

            $.ajax({
                type: "POST",
                url: '@Url.Action("Submit")',
                data: formData,
                beforeSend: function() {
                    $("#loader").show();
                },
                complete: function() {
                    $("#loader").hide();
                },
                success: function(response) {
                    if (response.status) {
                        // Close modal
                        $("#staticBackdropLargeScrollable").modal('hide');

                        // Show success message
                        Swal.fire({
                            icon: 'success',
                            title: 'Berhasil',
                            text: response.message
                        }).then(function() {
                            // Redirect to Role index
                            window.location.href = '@Url.Action("Index", "Role")';
                        });
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: response.message
                        });
                    }
                },
                error: function() {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Terjadi kesalahan sistem'
                    });
                }
            });
        });
    });
</script>