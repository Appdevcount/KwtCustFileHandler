using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;

namespace WebApplication1.Models
{
    public class Upolad
    {
       public int userid { get; set; }

    }
    

    public class Filedetails
    {

        //  public string Filename { get; set; }

        //        public int Fileid { get; set; }


        public string Fileid { get; set; }
        public string Filename { get; set; }

        public string shortFileName { get; set; }
       // public string DocumentName { get; set; }
        public string DocumentType { get; set; }

      public string Uploadeddate { get; set; }
      public string description  { get; set; }

        public string createdBy { get; set; }

    }

    public class TokenvalueDetails
    {
        public int DocumentId { get; set; }
        public int ProfileReferenceId { get; set; }
        public int sProfileReferenceId { get; set; }

        public string LanguageId { get; set; }

        public string DocumentName { get; set; }
        public string NewFileName { get; set; }
        public string StateId { get; set; }
        public string RoleID { get; set; }
        public string Description { get; set; }
        public string DateCreated { get; set; }
        public string ReferenceType { get; set; }
        public string ReferenceProfile { get; set; }
        public string DeclarationDocumentType { get; set; }
        public string DeclarationId { get; set; }
        public string ReqCreatedRole { get; set; }
        public string ScanDocRequestId { get; set; }
        public string RecCreatedBy { get; set; }
     
        public string profileName { get; set; }
        public string TablePrimaryKey { get; set; }
        public string UploadedFrom { get; set; }

    }


    public class ddlDocumentTypes
    {
        public int Value { get; set; }

        public int selectedValue { get; set; }


        public string Text  { get; set; }

    }

    public class FileModel
    {
        public string IEVersion { get; set; }

        public string isbroker { get; set; }
        public string tokencreatedby { get; set; }

        public string DeclarationId { get; set; }
        public string islocked { get; set; }

        
        public string IsUploadLocked { get; set; }

        // for item association 
        public string ItemAssocitaionFlag { get; set; }
        public string ItemAssocitaionprofilename { get; set; }


        public int SelectedOrderId { get; set; }

        public string disabled { get; set; }

        public string UploadStatus { get; set; }

        public bool filesize { get; set; }

        public string DeletedStatus { get; set; }
       // [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Document Name *")]

        public HttpPostedFileBase[] files { get; set; }

        public string SelectedFileId { get; set; }
        public List<Filedetails> listofuploadedFiles { get; set; }

        public List<CommercialInvoiceDetails> listofCommercialInvoiceItems { get; set; }
      
        
        public List<SelectListItem> ddlDocumentTypesitems { get; set; }
       //  public int? docsid { get; set; }

       public string docsid { get; set; }


        public string  text { get; set; }

        public string SelectedDropdownId { get; set; }

        //  public int SelectedDropdownId { get; set; }

        public string Description { get; set; }

        public string languagesession { get; set; }
        public int rowCount { get; set; }
        // for commercial
        public string SelectedItemAssocationId { get; set; }
        
        
              public string SelectedItemAssocationdocumentid { get; set; }

        // for gcs refund process
        public string ReferenceNo { get; set; }


        public string ReferenceCaption { get; set; }

    }

    public class CommercialInvoiceDetails
    {

        public int CIIID { get; set; }
        public string HSCode { get; set; }

        public string Origin { get; set; }
        public string GoodsDescription { get; set; }
        public string Manufacturer { get; set; }
        public string InvoiceNo { get; set; }
        public int Quantity { get; set; }
        public int NoOfPackages { get; set; }
        public int NetWeight { get; set; }
        public int GrossWeight { get; set; }
        public string Selected { get; set; }

        public string itemNumber { get; set; }


    }

}