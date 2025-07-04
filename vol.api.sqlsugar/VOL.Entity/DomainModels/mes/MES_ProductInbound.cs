/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using VOL.Entity.SystemModels;

namespace VOL.Entity.DomainModels
{
    [Entity(TableCnName = "产品入库", TableName = "MES_ProductInbound", DBServer = "ServiceDbContext")]
    public partial class MES_ProductInbound : BaseEntity
    {
        /// <summary>
        ///入库ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        [Key]
        [Display(Name = "入库ID")]
        [MaxLength(36)]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public Guid InboundID { get; set; }

        /// <summary>
        ///单据号
        /// </summary>
        [Display(Name = "单据号")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string DocumentNo { get; set; }

        /// <summary>
        ///物料名称
        /// </summary>
        [Display(Name = "物料名称")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string MaterialName { get; set; }

        /// <summary>
        ///物料编号
        /// </summary>
        [Display(Name = "物料编号")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string MaterialCode { get; set; }

        /// <summary>
        ///规格型号
        /// </summary>
        [Display(Name = "规格型号")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string SpecificationModel { get; set; }

        /// <summary>
        ///仓库
        /// </summary>
        [Display(Name = "仓库")]
        [MaxLength(36)]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? WarehouseID { get; set; }

        /// <summary>
        ///货位
        /// </summary>
        [Display(Name = "货位")]
        [MaxLength(36)]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        public Guid? LocationID { get; set; }

        /// <summary>
        ///入库数量
        /// </summary>
        [Display(Name = "入库数量")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int InboundQuantity { get; set; }

        /// <summary>
        ///入库单位
        /// </summary>
        [Display(Name = "入库单位")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string InboundUnit { get; set; }

        /// <summary>
        ///入库日期
        /// </summary>
        [Display(Name = "入库日期")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? InboundDate { get; set; }

        /// <summary>
        ///入库操作员
        /// </summary>
        [Display(Name = "入库操作员")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string InboundOperator { get; set; }

        /// <summary>
        ///入库原因
        /// </summary>
        [Display(Name = "入库原因")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string InboundReason { get; set; }

        /// <summary>
        ///创建人ID
        /// </summary>
        [Display(Name = "创建人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? CreateID { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [Display(Name = "创建人")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string Creator { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///修改人ID
        /// </summary>
        [Display(Name = "修改人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? ModifyID { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string Modifier { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? ModifyDate { get; set; }
    }
}
