// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// dashboard.js

// Function to show edit modal

        // Function to show edit modal
    function showEditModal(slotId) {
        console.log('showEditModal called with slotId:', slotId); // Check if function is called and slotId is correct
    $.get('/DoctorDashboard/EditSlotModal', {id: slotId })
    .done(function (data) {
        console.log('Received data from EditSlotModal:', data); // Check the data received
    $('#editSlotModalContent').html(data);
    $('#editModal').show();
                })
    .fail(function () {
        alert('Failed to load edit modal.');
                });
        }


    // Function to show delete modal
    function showDeleteModal(slotId) {
        console.log('showDeleteModal called with slotId:', slotId); // Check if function is called and slotId is correct
    $.get('/DoctorDashboard/DeleteSlotModal', {id: slotId })
    .done(function (data) {
        console.log('Received data from DeleteSlotModal:', data); // Check the data received
    $('#deleteSlotModalContent').html(data);
    $('#deleteModal').show();
                })
    .fail(function () {
        alert('Failed to load delete modal.');
                });
        }


    // Function to hide modal
    function hideModal(modalId) {
        $('#' + modalId).hide();
        }

    // Attach click event handlers
    $(document).ready(function () {
        $('.edit-btn').click(function () {
            var slotId = $(this).data('id');
            showEditModal(slotId);
        });

    $('.delete-btn').click(function () {
                var slotId = $(this).data('id');
    showDeleteModal(slotId);
            });
        });
