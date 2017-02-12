BaseURL = window.location.origin;
$(document).ready(function () {
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
              prompt: 'Please select pet'
          }
        ]
    },
    date: {
        identifier: 'date',
        rules: [
          {
              type: 'empty',
              prompt: 'Please select Date'
          }
        ]
    },
    detail: {
        identifier: 'detail',
        rules: [
          {
              type: 'empty',
              prompt: 'Please input detail'
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
                prompt: 'Please input pet name'
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
                    alert("Crete success");
                    window.location.reload();
                } else {
                    alert(data.Result);
                    $('.modal').modal('hide');
                }
            })

        },
    }

    var formSettingsPet =
{
    onSuccess: function (e) {
        e.preventDefault();
        alert("Crete success");
        $.post(BaseURL + '/VA/Member/CreatePet', {
            memberID: $('#member').val(),
            petType: $('#select_specie').val(),
            petName: $('#petName').val()
        }, function (data) {
            if (data.Result == "Success") {
                alert("Crete success");
                window.location.reload();
            } else {
                alert(data.Result);
                $('.modal').modal('hide');
            }
        })

    },
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



    $('.ui.dropdown')
      .dropdown()
    ;
    $('#select_condition')
.dropdown();

    $('.menu .item').tab();

    $('.search_button').on('click', function () {

        condition = $('#select_condition').val(),

            keyword = $('#search_keyword').val(),
        console.log("data is" + condition + " keyword is" + keyword);
        window.location.href = BaseURL + "/VA/Home/SearchMember/" + condition + "/" + keyword;
        /*   window.location.href = window.location.origin + "/Forums/Create/" + type;*/
    });


    $('.calendar_date').calendar({
        type: 'date'
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
        window.location.href = BaseURL + "/VA/Home/Index/" + "?month=" + month + "&year=" + year;
        },
    });


    $('.all_app_calendar').calendar({
        type: 'month',
        onChange: function (date) {
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
        window.location.href = BaseURL + "/VA/Home/Appointment/" + "?month=" + month + "&year=" + year;
        },
    });

    $('.test_app_calendar').calendar({
        type: 'date',
        onChange: function (date) {
            day = new Date(date).getDate(),
            month = new Date(date).getMonth() + 1,
            year = new Date(date).getFullYear(),
        window.location.href = BaseURL + "/VA/Home/" + "?day=" + day + "&month=" + month + "&year=" + year;
        },
    });


    $(".clickable-row").click(function () {
        window.location.href = BaseURL + "/VA/Member/Index/" + $(this).data('id');
    });


    $('.edit_app_button').click(function (e) {
        e.preventDefault();
        $.post(BaseURL + '/VA/Appointment/Edit', {
            appid: $(this).data('id')
        }, function (data) {
            if (data.Result == "Success") {
                alert("Delete success");
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

    $(".call_appWait_detail_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        appid: $(this).data('id')
        $('#appWait_detail_modal_' + thisObject.data('id')).modal('show');
    });

    $(".call_appComing_detail_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        appid: $(this).data('id')
        $('#appComing_detail_modal_' + thisObject.data('id')).modal('show');
    });

    $(".call_edit_app_modal").click(function (e) {
        e.preventDefault();
        var thisObject = $(this);
        appid: $(this).data('id')
        $('#edit_app_detail_modal_' + thisObject.data('id')).modal({
            onShow: function() {
                $('.calendar_date').calendar({
                    type: 'date'
                })
            },
            detachable: true,
                closable: false,
                onApprove: function() {
                    $('#edit_app_form').submit(); //Return false as to not close modal dialog
                    return false;
                }
            }
        ).modal('show');;
    });



});