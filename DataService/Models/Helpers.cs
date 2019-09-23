using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Models
{
    public static class ConstantManager
    {
        public const int STT_SUCCESS = 0;
        public const int STT_FAIL = 1;
        public const int STT_MISSING_PARAM = 2;
        public const int STT_UNAUTHORIZED = 3;
        public const int STATUS_SUCCESS = 200;
        public const int NUMBER_BLOG_POST = 20;
        public const string MES_LOGIN_SUCCESS = "Login successfully";
        public const string MES_LOGIN_FAIL = "Login fail";
        public const string MES_INVALID_LOGIN_ATTEMP = "Invalid login attempt.";
        public const string MES_WRONG_USERNAME_OR_PASSWORD = "Sai tên đăng nhập hoặc mật khẩu";
        public const string MES_NO_STORE__ASSIGNED_USERNAME = "Tài khoản của bạn chưa thuộc bất kỳ cửa hàng nào. Xin vui lòng liên hệ Quản trị viên để được giúp đỡ";
        public const string MES_NOTIFICATION_SUCCESS = "return notification success";
        public const string MES_NOTIFICATION_FAIL = "return notification fail";
        public const string MES_EDITACOUNTPHONE_FAIL = "Kết nối thất bại";
        public const string MES_EDITACOUNTPHONE_CUSTOMER_FAIL = "Could not find customer";
        public const string MES_CONNECTFB_VALID = "Mã định dạng sai";
        public const string MES_CONNECTFB_ISTHERE = "Không thể cập nhật. \nTài khoản đã được đăng ký!";
        public const string MES_CONNECTACCOUTNKIT_ISTHERE = "Không thể cập nhật. \nSố điện đã được đăng ký!";
        public const string MES_TRANSACTION_OUT = "Tạo Customer bị lỗi transaction";
        public const string MES_CREATE_DELIVERY_SUCCESS = "Tạo địa chỉ giao hàng thành công";
        public const string MES_CREATE_DELIVERY_FAIL = "Tạo địa chỉ giao hàng thất bại";
        public const string MES_DELIVERY_MAX = "Không thể tạo quá 3 địa chỉ";
        public const string MES_DELIVERYID_WRONG = "DeliveyId không đúng";
        public const string MES_DELETE_DELIVERY_SUCCESS = "Delete Delivery thành công";
        public const string MES_DELETE_DELIVERY_FAIL = "Delete Delivery thất bại";
        public const string MES_ORDERTYPE_NOTSUPPORT = "Loại đơn hàng không hỗ trợ";
        public const string MES_RATINGPRODUCT_NOTEXIST = "Rating product không tồn tại";
        public const string MES_PRODUCTBRAND_NOTFOUND = "Không tìm thấy thương hiệu";
        public const string MES_PRODUCTBRAND_OK = " Tìm thương hiệu thành công";
        public const string MES_CREATE_PRODUCT_BRAND_SUCCESS = " Tạo product brand thành công";
        public const string PRODUCT_BRAND_UPDATE_FAIL = "Update brand fail";
        public const string MES_UPDATE_PRODUCT_BRAND_SUCCESS = "Update brand thành công";
        public const string MES_EDITACOUNTPHONE_SUCCESS = "Kết nối thành công";
        public const string MES_UPDATECUSTOMER_FAIL = " update account phone fail";
        public const string MES_UPDATE_SUCCESS = "Update successfully";
        public const string MES_CONNECTFB_FAIL = "Kết nối thất bại";
        public const string MES_UPDATE_FAIL = "Update fail";
        public const string MES_MISSING_PARAM = "Missing parameter";
        public const string MES_UNVALID_TOKEN = "Token is unvalid or expired";
        public const string MES_MISSING_IMAGE = "Missing your image";
        public const string MES_RATING_CREATE_SUCCESS = "Xếp hạng thành công";
        public const string MES_RATING_CREATE_FAIL = "Xếp hạng xảy ra lỗi";
        public const string MES_RATINGPRODUCT_UPDATE_SUCCESS = "Update Rating Product Success";
        public const string MES_PRODUCTCATEGORY_NOT_FOUND = "Không tìm thấy productCategory ";
        //check role khi xem store
        public const string MES_STORE_UNAUTHENTICATED = "You do not have permission to see report of this store";
        // check role khi nhan token
        public const string MES_ROLE_UNAUTHENTICATED = "You do not have permission";
        public const string MES_LOAD_REPORT_SUCCESS = "Load report success";
        public const string MES_FAIL = "Return fail";
        public const string MES_UPDATE_DELIVERYINFO = "Existed least 1 default delivery info";
        public const string MES_FULL_DELIVERY = "Full deliveryInfo";
        public const string MES_SUCCESS = "Return success";
        public const string ROLE_ADMIN = "Administrator";
        public const string ROLE_MANAGER = "StoreManager";
        
        public const string MES_PROMOTION_FAIL = "Voucher không hợp lệ";
        public const string MES_PROMOTION_DATE_FAIL = "Không áp dụng với khung giờ hiện tại";
        public const string MES_PROMOTION_SUCCESS = "Chúc mừng! Mã giảm giá hợp lệ";
        public const string MES_STORE_FAIL = "Don't have store";
        public const string MES_STORE_SUCCESS = "Success";
        public const string MES_PAYMENT_CASH = "Thanh toán bằng tiền mặt";
        public const string MES_PAYMENT_MOMO = "Thanh toán qua MomMo";
        public const string MES_UPDATE_PAYMENT_FAIL = "Update payment không thành công";
        public const string MES_PAYMENTTYPE_NOTSUPPORT = "Không hỗ trợ phương thức thanh toán này";
        public const string MES_CHECK_CUSTOMERID_FAIL = "Không tìm thấy khách hàng";
        public const string MES_CHECK_CUSTOMERDEVICE_FAIL = "Không thể cập nhật.";
        public const string MES_CHECK_CUSTOMERDEVICE_SUCCESS = "Cập nhật thành công token device.";
        public const string MES_CREATE_ORDER_FAIL = "Tạo đơn hàng không thành công";
        public const string MES_CREATE_ORDER_NOT_FOUND_VOUCHER = "Voucher không tồn tại hoặc đã được sử dụng";
        public const string MES_CREATE_ORDER_NEGATIVE_QUANTITY = "Order detail quantity bị âm";
        public const string MES_ORDER_HISTORY_NOTFOUND = "Order History Not Found";
        public const string MES_CREATE_ORDER_NOT_FOUND_PRODUCT = "Không tìm thấy sản phẩm";
        public const string MES_CREATE_ORDER_EXCEED_VOUCHER_MIN_MAX = "Số tiền vượt quá giới hạn voucher";
        public const string MES_UPDATE_ACCOUNT_FAIL = "Tài khoản không đủ";
        public const string MES_PRODUCT_NOTFOUND = "Không tìm thấy sản phẩm";
        public const string MES_REQUEST_DENY = "Không có quyền thực hiện yêu cầu này";
        public const string MES_RATINGPRODUCT_NOTFOUNT = "Không có rating";
        public const string MES_RATINGPRODUCT_OK = "Tìm thấy rating";
        public const string MES_PRODUCT_NOTEXIST = "Sản phẩm không tồn tại";
        public const string MES_CUSTOMER_PHONE_EXIST = "Số điện thoại đã được tài khoản khác đăng kí";
        public const string MES_CUSTOMER_EMAIL_EXIST = "Địa chỉ email đã được tài khoản khác đăng kí";
        public const string MES_CUSTOMER_EMAIL_NOTFOUND = "Không tìm thấy địa chỉ email trong access_token";
        public const string MES_CUSTOMER_NOTFOUND = "Không tìm thấy thấy khách hàng";
        public const string MES_PARENT_ORDERDETAIL_NOTFOUND = "Không tìm thấy chi tiết đơn hàng sản phẩm chính";
        public const string MES_CHILD_ORDERDETAIL_WRONG = "Sản phẩm thêm không thuộc sản phẩm chính";
        public const string MES_EXTRA_MAPPING_NOTFOUND = "Extra mapping không tìm thấy";
        public const string MES_RATE_VALID = "Không thể xếp hạng cho sản phẩm và đơn hàng cùng lúc";
        public const string MES_ORDER_NOT_FOUND = "Không thể tìm thấy đơn hàng";

        public const string MES_CREATE_ORDER_SUCCESS = "Tạo đơn hàng thành công";
        public const string MES_CREATE_PAYMENT_MOMO_SUCCESS = "Thanh toán với momo thành công";
        public const string MES_CREATE_PAYMENT_MOMO_FAIL = "Thanh toán với momo thất bại";
        public const string PRIVATE_KEY = "WiskySRKey";
        public const string PREFIX_MOBILE = "MB";
        public const int MAX_RECORD = 25;
        public const string DELIVERY = "DELIVERY";
        public const int MAX_DELIVERYINFO = 3;
        public const string PRODUCT_CATEGORY_EVENT = "EVENT";
        public const string PRODUCT_CATEGORY_OTHER = "KHÁC";
        public const string PRODUCT_CATEGORY_ITALI = "ITALI PANINI";
        public const string PRODUCT_CATEGORY_FRESH_JUICE = "FRESH JUICE";
        public const string PRODUCT_CATEGORY_CAKE = "FRANK CAKE";
        public const string PRODUCT_CATEGORY_COMBO = "COMBO";
        public const string PRODUCT_CATEGORY_SPARKLING = "SPARKLING";
        public const string PRODUCT_CATEGORY_PASSIO_COFFEE = "PASSIO COFFEE";
        public const string PRODUCT_BRAND_CREATE_FAIL = "Tạo mới product brand thất bại";
        public const string IMAGE_SERVER_PATH = "D:\\PublicIIS\\MobileAPI\\ServerImage";
        public const string IMAGE_SERVER_URL = "http://115.165.166.32:30779/ServerImage";
        public const string IMAGE_UNI_SERVER_PATH = "E:\\PublicIIS\\MobileReso_APIDev\\ServerImage";
        public const string IMAGE_UNI_SERVER_URL = "http://210.2.93.10:42018/ServerImage";
        public const string IMAGE_FORMAT_EXTENSION = ".Png";
        public const string MESS_ORDER_NEW = "Đang chờ confirm";
        public const string FORMART_DATETIME = "yyyy-MM-ddTHH:mm";
        public const string FORMART_DATETIME_2 = "dd/MM/yyyy";
        public const string PARTNERCODEMOMO = "MOMOIQA420180417";
        public const int CALLCENTER_ID = 3;
        public const double DELIVERY_FREE = 50000;
        public const double DISCOUNT_VOUCHER = 100000;
        public const string MES_CHECK_VOUCHER_RULE_1 = "Áp dụng cho đơn hàng trên ";
        public const string MES_CHECK_VOUCHER_RULE_2 = "Áp dụng cho đơn hàng khi mua  ";
        public const int PROMOTION_RULE_1 = 1;//rule min order
        public const int PROMOTION_RULE_2 = 2;//rule discount each product
        public const string PROMOTION_CASH_BACK = "Cash Back"; // promotion cash back for membership 
        public const double MEMBER_POINT_AMOUNT = 10000;
        public const string NOTIFICATION_URI = "http://fcm.googleapis.com/fcm/send";
        public static string APP_AUTHORIZATION = "key=AIzaSyD6dQmAfR1Zl6vwhe34poCnviuyPGhEcak";
        public static List<int> LIST_MOBILE_TYPE_ID = new List<int> { (int)MembershipCardTypeEnum.Newbie, (int)MembershipCardTypeEnum.Gold, (int)MembershipCardTypeEnum.Diamond };

    }
}
