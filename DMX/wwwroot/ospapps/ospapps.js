﻿$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $(' button[data-bs-toggle="ajax-modal"]').on('click', function (event) {
        var url = $(this).data('bs-url');
        var decodeUrl = decodeURI(url);
        $.get(decodeUrl).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');

        });
    });
});

$(function () {
    $('.table').DataTable({
        "order": [[0, 'desc']]
    });

    $('.table').DataTable().on('order.dt search.dt', function () {
            let i = 1;

           
        $('.table').DataTable()     .cells(null, 0, { search: 'applied', order: 'applied' })
                .every(function (cell) {
                    this.data(i++);
                });
        })
        .draw();
});



$(function ()
{
    $('.table').DataTable();
  
    $('#dataTableAtt').DataTable(
        {
            "lengthMenu": [[-1], ["All"]],
        });

    var table = document.getElementById('#dataTableRpt').DataTable({
        dom:
            "<'row'<'col-sm-4'l><'col-sm-4'B><'col-sm-4'f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-5'i><'col-sm-7'p>>",

        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
        /*    searchBuilder: true,*/

        //buttons: [
        //    {
        //        extend: 'collection',
        //        className: 'collectionButton',
        //        text: 'Export',

        //        buttons: [

        //            'pdf',
        //            'csv',
        //            'excel',
        //            'colvis'],      
        //    }],
        //buttons: ['searchBuilder']
        buttons: {
            buttons: [
                {
                    extend: 'colvis',
                    className: 'colvisButton',
                    text: 'Column Visibility',
                },

                {
                    extend: 'pdf',
                    className: 'pdfButton',
                    text: 'PDF',

                },

                {
                    extend: 'csv',
                    className: 'csvButton',
                    text: 'CSV',
                },
                {
                    extend: 'print',
                    className: 'printButton',
                    text: 'Print',
                },

                {
                    extend: 'searchBuilder',
                    className: 'searchBuilderButton',
                    text: 'Search Builder',
                },
            ]
        }




        //buttons: ['csv'],
        //buttons: ['excel'],
        //buttons: ['colvis'],
        //buttons: ['searchBuilder']

    });

    table.buttons().container().appendTo($('.card-header'));
    /*  table.searchBuilder.container().prependTo($('.card-header'));*/
    /* table.searchBuilder.container().prependTo(table.table().container());*/
});


$(function () {

$('time.timeago').timeago();
});

$(function () {

    $('.did-floating-select').select2();
});
$(function () {
    tinymce.init({
        selector: 'texterea',
      
        height: '400',
        plugins: [
            'advlist autolink lists link image charmap print preview anchor textcolor',
            'searchreplace visualblocks code fullscreen media Image',
            'insertdatetime media table contextmenu paste code help autoresize',],
        toolbar:
            'insert | undo redo | styleselect | bold italic backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | removeformat | help'
    });
    tinymce.activeEditor.setContent("MemoContent");
});
    
    $(function () {
    tinymce.init({
        selector: 'textarea#detail',
       
        readonly: true,
        menubar: false,
        statusbar: false,
        toolbar: false,
        plugins: 'autoresize',
    });
});
   
   
$(function () {
    $('#smartwizard').smartWizard({
        theme: 'arrows'
    });
});

   
$(function () {
    $("#btnAddMember").on('click', function (e) {
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, submit!',
            reverseButtons: true,
        }).then(result => {
            if (result.isConfirmed) {
                const memberForm = document.getElementById("AddMember");
                memberForm.submit();
                if (memberForm.submit.is) {
                    swal.fire('Saved!',
                        'Your form has been submitted.',
                        'success');
                }
                else {
                    swal.fire('Error!!!', 'Your form could not be submitted.', 'error')
                }
            }
        });
    });
});

$(function GetMessages() {
    $.ajax({
        type: "GET",
        url: "/Message/UserMessages",
        success: function (result) {
            $('.messagebox').html(result);
            $('#counter').html($('.message').length)
        },
    });
});


    $(document).on('click', '.message', function () {

        $(this).hide();
        $('#counter').html($('.message:not([style*="display: none"])').length);
        $.ajax({
            type: "POST",
            url: "/Message/UpdateMessage",
            data: { Id: $("#msgId").val() },
        });
        GetMessages();
    });
    function bankDetails() {
        let bankSection = document.getElementById('Bank');
        let momoSection = document.getElementById('Momo');
        let isShow = true;


        bankSection.classList.remove("d-none");
        bankSection.classList.add("d-block");
        momoSection.classList.remove("d-block");
        momoSection.classList.add("d-none");
    }
    function momoDetails() {
        bankSection.classList.remove("d-block");
        bankSection.classList.add("d-none");
        momoSection.classList.remove("d-none");
        momoSection.classList.add("d-block");
    }

jQueryAjaxDeleteMemo = form => {
    if (confirm('Are you sure to delete this Memo?'))
    {
        try
        {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
                    LoadMemos();
                
           
        }
        catch (e)
        {
            LoadMemos();
        }
    }
    return false;
}
$(function LoadMemos() {
    $.ajax({
        type: "GET",
        url: "/Memo/ViewMemos",
    
    });
});
jQueryAjaxDeletePatient = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadPatients() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});

jQueryAjaxDeleteLetter = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadLetters() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});

jQueryAjaxDeleteTravelRequest = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadTravelRequests() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});

jQueryAjaxDeleteServiceRequest = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadServiceRequests() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});

jQueryAjaxDeletePettyCash = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadPatients() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});

jQueryAjaxDeleteExcuseDuty = form => {
    if (confirm('Are you sure to delete this Patient?')) {
        try {
            $.ajax({
                type: 'POST',
                url: form.action,
            });
            LoadPatients();


        }
        catch (e) {
            LoadPatients();
        }
    }
    return false;
}
$(function LoadPatients() {
    $.ajax({
        type: "GET",
        url: "/Patient/Patients",

    });
});


