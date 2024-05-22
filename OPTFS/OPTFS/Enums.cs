public enum StudentRequestStatus
{
    New = 1, // 1- جديد 
    Viewed = 2,// 2- تم عرضه
    Opened = 3,// 3- تم فتحه
    Rejected = 4,// 4- تم رفضه
    Accepted = 5,// 5- تم قبوله
    Payed = 6,// 6- تم الدفع    
    Completed=7,
}

public enum NotificationType
{
    TutorNewRequest = 0, // 0- تنبيه للآدمن بطلب المعلم
    TutorRejected =1, // 1- تنبيه رفض المعلم
    TutorAccepted=2,// 2- تنبيه قبول المعلم
    StudentRequest=3,// 3- تنبيه للمعلم بطلب الطالب
    StudentRejected=4,// 4- تنبيه رفض الطالب
    StudentAccepted=5,// 5- تنبيه قبول الطالب    
    StudentPayed = 6, // 6- تنبيه للمعلم باكتمال الدفع    
}

public enum MessageStatus
{
    New=1,// جديد
    Delivered=2,// تم تسليمه
    Viewed=3,// تمت قرائته
    Deleted=4// محذوف
}