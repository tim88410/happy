//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SlnBlogAndShop
{
    using System;
    using System.Collections.Generic;
    
    public partial class tParentcomment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tParentcomment()
        {
            this.tChildcomments = new HashSet<tChildcomment>();
        }
    
        public int fCommentId { get; set; }
        public int fPostId { get; set; }
        public string fDescription { get; set; }
        public int fMemberId { get; set; }
        public System.DateTime fTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tChildcomment> tChildcomments { get; set; }
        public virtual tMember tMember { get; set; }
        public virtual tPost tPost { get; set; }
        public virtual tPost tPost1 { get; set; }
    }
}
