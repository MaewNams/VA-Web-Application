﻿BaseURL = window.location.origin;
$(document).ready(function () {


    $('#create_member_account')
       .form({
           name: {
               identifier: 'name',
               rules: [
                   {
                       type: 'empty',
                       prompt: 'Fail, name is required'
                   }
               ]
           },
           surname: {
               identifier: 'surname',
               rules: [
                   {
                       type: 'empty',
                       prompt: 'Fail, surname is required'
                   }
               ]
           },
           address: {
               identifier: 'address',
               rules: [
                   {
                       type: 'empty',
                       prompt: 'Fail, address is required'
                   }
               ]
           },
           phoneNumber: {
               identifier: 'phoneNumber',
               rules: [
                   {
                       type: 'empty',
                       prompt: 'Fail, phone number is required'
                   },
                   {
                       type: 'exactLength[10]',
                       prompt: 'Fail, phone number have to contain 10 numeric character'
                   },
                   {
                       type: 'number',
                       prompt: 'Fail, phone number can only contain 0-9'
                   }
               ]
           }

       }, {
           on: 'blur',
           inline: false,
           onSuccess: function (e) {
               e.preventDefault();
               $.post(BaseURL + '/VA/Member/CreateMember', {
                   name: $('#name').val(),
                   surname: $('#surname').val(),
                   address: $('#address').val(),
                   phoneNumber: $('#phoneNumber').val(),
               }, function (data) {
                   if (data.Result == "Success") {
                       alert("Crete success");
                       window.location.href = BaseURL + "/VA/Member/Index/" + data.newMemberId;
                   } else {
                       alert(data.Result);
                   }
               })

           },
           onFailure: function () {
               return false;
           }
       });

    /*create member account*/
    $(".call_create_member_modal").click(function (e) {
        e.preventDefault();
        $('#create_member_modal').modal({
            detachable: true,
            closable: false,
        }).modal('show');;
    });

    $('.confirm_create_member_button').click(function (e) {
        e.preventDefault();
        var appid = $(this).attr("value");
        $.post(BaseURL + '/VA/Member/CreateMember',
                {
                    name: $('#create_name').val(),
                    surname: $('#create_surname').val(),
                    address: $('#create_address').val(),
                    phoneNumber: $('#create_phoneNumber').val()

                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Create success");
                        window.location.href = BaseURL + "/VA/Member/Index/" + data.newMemberId;
                    } else {
                        alert(data.Result);
                    }
                })

    });

    /*edit member account*/
    $(".call_edit_member_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        petid: $(this).data('id')
        $('#edit_member_modal').modal({
            detachable: true,
            closable: false,
        }).modal('show');;
    });


    $('.get_new_password').click(function (e) {
        var newRandomPassword = "";
        e.preventDefault();
        $.post(BaseURL + '/VA/Member/GetRandomPassword',
                function (data) {
                    if (data.Result == "Success") {
                        newRandomPassword = data.newPassword;
                        $('input[name=password]').val(newRandomPassword);
                    } else {
                        alert(data.Result);
                    }
                })

    });


    $('.confirm_edit_member_button').click(function (e) {
        e.preventDefault();
        var appid = $(this).attr("value");
        $.post(BaseURL + '/VA/Member/Edit',
                {
                    id: $('#edit_member_id').val(),
                    name: $('#edit_name').val(),
                    surname: $('#edit_surname').val(),
                    address: $('#edit_address').val(),
                    phonenumber: $('#edit_phoneNumber').val(),
                    password: $('#edit_password').val()

                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                    }
                })

    });

    /*edit app*/
    $(".call_edit_app_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        petid: $(this).data('id')
        $('#edit_app_modal_' + thisObject.data('id')).modal({
            detachable: true,
            closable: false,
        }).modal('show');;
    });

    $('.confirm_edit_app_button').click(function (e) {
        e.preventDefault();
        var appid = $(this).attr("value");
        $.post(BaseURL + '/VA/Appointment/Edit',
                {
                    appid: $('#edit_app_' + appid).val(),
                    detail: $('#edit_detail_' + appid).val(),
                    suggestion: $('#edit_suggestion_' + appid).val()

                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                    }
                })

    });

    /*edit app month*/
    $(".call_edit_mApp_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        petid: $(this).data('id')
        $('#edit_mApp_modal_' + thisObject.data('id')).modal({
            detachable: true,
            closable: false,
        }).modal('show');;
    });

    $('.confirm_edit_mApp_button').click(function (e) {
        e.preventDefault();
        var appid = $(this).attr("value");
        $.post(BaseURL + '/VA/Appointment/Edit',
                {
                    appid: $('#edit_mApp_' + appid).val(),
                    detail: $('#edit_mApp_detail_' + appid).val(),
                    suggestion: $('#edit_mApp_suggestion_' + appid).val()

                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                    }
                })

    });

    /**/

    $(".call_edit_pet_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        petid: $(this).data('id')
        $('#edit_pet_modal_' + thisObject.data('id')).modal({
            detachable: true,
            closable: false,
        }).modal('show');;
    });


    $('.confirm_edit_pet_button').click(function (e) {
        e.preventDefault();
        var petid = $(this).attr("value");
        console.log(petid);
        console.log("type" + $('#select_edit_specie_' + petid));
        console.log("name" + $('#edit_petName_' + petid));
        $.post(BaseURL + '/VA/Member/EditPet',
                {
                    petID: $('#edit_pet_' + petid).val(),
                    petType: $('#select_edit_specie_' + petid).val(),
                    petName: $('#edit_petName_' + petid).val(),

                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                    }
                })

    });




    $('#create_app_modal')
    .modal({
        detachable: true,
        closable: false,
        onApprove: function () {
            $('#create_app_form').submit(); //Return false as to not close modal dialog
            return false;
        }

    })
    ;

    $('#create_pet_modal').modal({
        detachable: true,
        closable: false,
        transition: 'fade up',
        onApprove: function () {
            $('#create_pet_form').submit();//Return false as to not close modal dialog
            return false;
        }
    })
    ;


    var formValidationRulesApp =
{
    pet: {
        identifier: 'pet',
        rules: [
          {
              type: 'empty',
              prompt: 'Fail, pet name is required'
          }
        ]
    },
    date: {
        identifier: 'date',
        rules: [
          {
              type: 'empty',
              prompt: 'Fail, appointment date is required'
          }
        ]
    },
    detail: {
        identifier: 'detail',
        rules: [
          {
              type: 'empty',
              prompt: 'Fail, appointment detail is required'
          }
        ]
    }
}

    var formValidationRulesPet =
{
    petName: {
        identifier: 'petName',
        rules: [
            {
                type: 'empty',
                prompt: 'Fail, pet name is required'
            }
        ]
    }
}

    var formSettingsApp =
    {
        onSuccess: function (e) {
            e.preventDefault();
            $.post(BaseURL + '/VA/Member/CreateApp', {
                memberID: $('#member').val(),
                petId: $('#select_pet').val(),
                detail: $('#detail').val(),
                suggestion: $('#suggestion').val(),
                date: $('#date').val(),
            }, function (data) {
                if (data.Result == "Success") {
                    alert("Create success");
                    window.location.reload();
                } else {
                    alert(data.Result);
                    $('.modal').modal('hide');
                }
            })

        }
    }

    var formSettingsPet =
{
    onSuccess: function (e) {
        e.preventDefault();
        $.post(BaseURL + '/VA/Member/CreatePet', {
            memberID: $('#member').val(),
            petType: $('#select_specie').val(),
            petName: $('#petName').val()
        }, function (data) {
            if (data.Result == "Success") {
                alert("Create success");
                window.location.reload();
            } else {
                $('#create_pet_form').form('add prompt', 'petName');
                $('.form .error.message').html(data.Result).show();
            }
        })

    }
}


    $('#create_app_form').form(formValidationRulesApp, formSettingsApp);
    $('#create_pet_form').form(formValidationRulesPet, formSettingsPet);

    $('#call_create_app_modal').click(function () {
        $('.ui.form').trigger("reset");
        //Resets form error messages
        $('.ui.form .field.error').removeClass("error");
        $('.ui.form.error').removeClass("error");
        $('#create_app_modal').modal('show');
    });

    $('#call_create_pet_modal').click(function () {
        $('.ui.form').trigger("reset");
        //Resets form error messages
        $('.ui.form .field.error').removeClass("error");
        $('.ui.form.error').removeClass("error");
        $('#create_pet_modal').modal('show');
    });


    $('.appointment').click(function () {
        $('.active').removeClass('active');
        $(this).addClass('active');
    });


    $('.ui.dropdown')
      .dropdown()
    ;
    $('#select_condition').dropdown();

    $('.menu .item').tab({
    }
        );

    $('.search_button').on('click', function () {

        condition = $('#select_condition').val(),

            keyword = $('#search_keyword').val(),
        console.log("data is" + condition + " keyword is" + keyword);
        window.location.href = BaseURL + "/VA/home/member/" + condition + "/" + keyword;
        /*   window.location.href = window.location.origin + "/Forums/Create/" + type;*/
    });

    $('.calendar_date').calendar({
        type: 'date',
        minDate: new Date((new Date()).valueOf() + 1000 * 3600 * 24),
        maxDate: null
    });

    $('.time_only').calendar({
        type: 'time'
    });

    $('.example7').calendar({
        type: 'month',
        onChange: function (date) {
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
         memberID = $('#member').val(),
        window.location.href = BaseURL + "/VA/Member/Index/" + memberID + "?month=" + month + "&year=" + year;
        },
    });

    $('.indexCalenda').calendar({
        type: 'month',
        onChange: function (date) {
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
        window.location.href = BaseURL + "/VA/" + "?month=" + month + "&year=" + year;
        },
    });

    $('#example14').calendar({
        type: 'date',
        inline: true,
        disableMinute: true,
    });
    $('.all_app_calendar').calendar({
        type: 'month',
        onChange: function (date) {
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
        window.location.href = BaseURL + "/VA/Home/Monthly/" + "?month=" + month + "&year=" + year;
        },
    });

    $('.test_app_calendar').calendar({
        type: 'date',
        onChange: function (date) {
            day = new Date(date).getDate(),
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
        window.location.href = BaseURL + "/VA/Home/Appointment" + "?day=" + day + "&month=" + month + "&year=" + year;
        },
    });


    $(".clickable-row").click(function () {
        window.location.href = BaseURL + "/VA/Member/Index/" + $(this).data('id');
    });

    $(".reset_data_button").click(function (e) {
        e.preventDefault();
        $.post(BaseURL + '/VA/Service/TruncateDB', {}, function (data) {
            if (data.Result == "Success") {
                alert("Reset data success");
                window.location.reload();
            } else {
                alert(data.Result);
            }
        })

    });

    $(".create_new_member_button").click(function () {
        $('#create_member_account').toggle("show");
    });


    $('.edit_app_button').click(function (e) {
        e.preventDefault();
        $.post(BaseURL + '/VA/Appointment/Edit', {
            appid: $(this).data('id'),
            detail: $('#edit_detail').val(),
            suggestion: $('#edit_suggestion').val(),
        }, function (data) {
            if (data.Result == "Success") {
                alert("Edit success");
                window.location.reload();
            } else {
                alert(data.Result);
            }
        })

    });



    $('.delete_app_button').click(function (e) {
        e.preventDefault();
        if (confirm("Do you want to delete appointment?")) {
            $.post(BaseURL + '/VA/Appointment/Delete', {
                appid: $(this).data('id')
            }, function (data) {
                if (data.Result == "Success") {
                    alert("Delete success");
                    window.location.reload();
                } else {
                    alert(data.Result);
                }
            })
        }
    });
    $('.delete_pet_button').click(function (e) {
        e.preventDefault();
        if (confirm("Do you want to delete this pet?")) {
            $.post(BaseURL + '/VA/Member/DeletePet', {
                petid: $(this).data('id')
            }, function (data) {
                if (data.Result == "Success") {
                    alert("Delete success");
                    window.location.reload();
                } else {
                    alert(data.Result);
                }
            })
        }
    });
    /*  $('#call_edit_app_modal').click(function () {
          $('.ui.form').trigger("reset");
          //Resets form error messages
          $('.ui.form .field.error').removeClass("error");
          $('.ui.form.error').removeClass("error");
  
          $('#create_app_modal').modal('show');
      });
      */

    $('.ui.menu .item')
  .tab({
      history: true,
      historyType: 'hash'
  });
    $('.edit_member_button').click(function (e) {
        e.preventDefault();
        if (confirm("Do you want to edit this member account?")) {
            $.post(BaseURL + '/VA/Member/Edit',
                {
                    id: $(this).data('id'),
                    name: $('#name').val(),
                    surname: $('#surname').val(),
                    address: $('#address').val(),
                    phonenumber: $('#phonenumber').val()
                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                        window.location.reload();
                    }
                })
        }
    });

    /*Call app detail modal*/

    $(".call_Mapp_detail_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        appid: $(this).data('id')
        $('#MappWait_detail_modal_' + thisObject.data('id')).modal('setting', 'closable', false).modal('show');
    });

    $(".call_app_detail_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        appid: $(this).data('id')
        $('#appWait_detail_modal_' + thisObject.data('id')).modal('setting', 'closable', false).modal('show');
    });

    /*    $(".call_edit_app_modal").click(function (e) {
            e.preventDefault();
            var thisObject = $(this);
            appid: $(this).data('id')
            $('#edit_app_detail_modal_' + thisObject.data('id')).modal({
                onShow: function () {
    
                    $('.calendar_date').calendar({
                        type: 'date',
                    })
                },
                detachable: true,
                closable: false,
                onApprove: function () {
                    $('#edit_app_form').submit(); //Return false as to not close modal dialog
                    return false;
                }
            }
            ).modal('show');;
        });
    
    
        */



    $('.edit_VA_Profile_button').click(function (e) {

        e.preventDefault();
        if (confirm("Do you want to edit Clinic detail?")) {
            $.post(BaseURL + '/VA/Home/EditClinic',
                {
                    address: $('#address').val(),
                    phoneNumber: $('#phoneNumber').val(),
                    open: $('#open').val(),
                },
                function (data) {
                    if (data.Result == "Success") {
                        alert("Edit success");
                        window.location.reload();
                    } else {
                        alert(data.Result);
                        window.location.reload();
                    }
                })
        }
    });


    $('.delete_member_button').click(function (e) {
        e.preventDefault();
        if (confirm("Do you want to delete this member account?")) {
            $.post(BaseURL + '/VA/Member/Delete', {
                memberid: $(this).data('id')
            }, function (data) {
                if (data.Result == "Success") {
                    alert("Delete success");
                    window.location.href = BaseURL + "/VA/Home/Member/";
                } else {
                    alert(data.Result);
                }
            })
        }
    });

});