namespace NationalBank.FrontEnd.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}


/*import Swal from 'sweetalert2';
import * as $ from 'jquery';*/


/*export class Sweetalert
{
    InitOnload() : void {
        $('#getdetails').on('click', () => {
        Swal.fire({
        title: "Do you want to save the changes?",
                showDenyButton: true,
                showCancelButton: true,
                confirmButtonText: "Save",
                denyButtonText: `Don't save`
            }).then((result) => {
            *//* Read more about isConfirmed, isDenied below *//*
            if (result.isConfirmed)
            {
                Swal.fire("Saved!", "", "success");
            }
            else if (result.isDenied)
            {
                Swal.fire("Changes are not saved", "", "info");
            }
        });
    })
    }


}*/

