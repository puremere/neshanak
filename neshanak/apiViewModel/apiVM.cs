﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace neshanak.apiViewModel
{
    public class basemodel
    {
        public string mbrand { get; set; }
    }
    public class addToWishList
    {
        public string mbrand { get; set; }
        public string id { get; set; }
        public string token { get; set; }
        public string status { get; set; }
    }
    public class getCats
    {
        public string mbrand { get; set; }
        public string ID { get; set; }

        public string catLevel { get; set; }
    }
    public class addTransaction
    {
        public string mbrand { get; set; }
        public string price { get; set; }
        public string token { get; set; }
        public string status { get; set; }
    }
    public class buyRequest
    {
        public string mbrand { get; set; }
        public string token { get; set; }
        public string status { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string addressID { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string hourID { get; set; }
        public string comment { get; set; }
        public string phone { get; set; }
        public string payment { get; set; }
        public string ids { get; set; }
        public string nums { get; set; }
        public string discount { get; set; }
        public string postalCode { get; set; }
        public string auth { get; set; }
    }

    public class callMe
    {
        public string mbrand { get; set; }
        public string id { get; set; }
        public string token { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }
    public class changePass
    {
        public string mbrand { get; set; }
        public string password { get; set; }
        public string activate_code { get; set; }
        public string mobile { get; set; }
      
    }
    public class commentArticleProduct
    {
        public string mbrand { get; set; }
        public string comment { get; set; }
        public string MyProtokenperty { get; set; }
        public string ID { get; set; }
        public string title { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public string token { get; set; }
    }
    public class commentProduct
    {
        public string mbrand { get; set; }
        public string token { get; set; }
        public string ID { get; set; }
        public string title { get; set; }
        public string comment { get; set; }
    }
    public class compare
    {
        public string mbrand { get; set; }
        public  string productID { get; set; }
        public  string productID2 { get; set; }
      
    }
    
    public class compareSearch
    {
        public string mbrand { get; set; }
        public  string productID { get; set; }
        public  string word { get; set; }
     
    }
    
          public class completeProfile
    {
        public string mbrand { get; set; }
        public  string mobile { get; set; }
        public  string token { get; set; }
        public  string fullname { get; set; }
        public  string email { get; set; }
        public  string province { get; set; }
        public  string city { get; set; }
        public  string address { get; set; }
        public  string latitude { get; set; }
        public  string longitude { get; set; }
    }

    public class confirmUser
    {
        public string mbrand { get; set; }
        public  string mobile { get; set; }
    }
    public   class defaultAddress
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string id{get; set;}
    }
    public class doFinalCheck
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string auth{get; set;} public string amount{get; set;} public string refID{get; set;}
           public string paymentStatus{get; set;} public string payment{get; set;} public string isPayed{get; set;}
    }
    public class doSignIn
    {
        public string mbrand { get; set; }
        public string password{get; set;} public string phone{get; set;}
    }
    public class doSignUp
    {
        public string mbrand { get; set; }
        public string password{get; set;} public string phone{get; set;} public string moaref{get; set;}
    }
    public class doWalletFinalCheck
    {
        public string mbrand { get; set; }
        public string auth{get; set;}
    }
    public class editProduct
    {
        public string mbrand { get; set; }
        public string id{get; set;} public string token{get; set;} public string newPrice{get; set;} public string newTitle{get; set;} public string newDesc{get; set;} public string newDiscount{get; set;}
            public string newCount{get; set;} public string isOffer{get; set;} public string isSpecial{get; set;} public string isAvalible { get; set;} public string isActive{get; set;}
    }
    public class FinalizeOrder
    {
        public string mbrand { get; set; }
        public string deliverID{get; set;} public string desc{get; set;} public string ID{get; set;} public string status{get; set;}

    }
    public class getCredit
    {
        public string mbrand { get; set; }
        public string token{get; set;}
    }
    public class getCode
    {
        public string mbrand { get; set; }
        public string user{get; set;} public string activeCode{get; set;}
    }
    public class getDataArticleComment
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string ID{get; set;}
    }



    public class getDataArticlesDetail
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string id{get; set;}
    }

    public class getDataCatArticle
    {
        public string mbrand { get; set; }
        public string page{get; set;}
    }
    public class getDataCatArticlesList
    {
        public string mbrand { get; set; }
        public string page{get; set;} public string id{get; set;} public string hashtag{get; set;}

    }

    public class getDataComment
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string ID{get; set;}
    }
    public class getDataMyOrderDetails
    {
        public string mbrand { get; set; }
        public string ID {get; set;}
    }
    public class getDataMyOrders
    {
        public string mbrand { get; set; }
        public string token{get; set;}
    }
    public class getDataProductList0
    {
        public string mbrand { get; set; }
        public string wonder { get; set; }
        public string page{get; set;} public string colorIds{get; set;} public string filterIds{get; set;} public string min
           {get; set;} public string max{get; set;} public string hashtag{get; set;} public string sortID{get; set;} public string priorityID{get; set;} public string specificItem{get; set;} public string query{get; set;} public string catID{get; set;} public string catLevel{get; set;} public string isAvalible{get; set;}
    }
    public class getDataProfile
    {
        public string mbrand { get; set; }
        public string mobile{get; set;} public string token{get; set;}
    }
    public class getDataWishList
    {
        public string mbrand { get; set; }
        public string token{get; set;}
    }
    public class getDeliverCode
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string deliverCode{get; set;} public string tranID{get; set;} public string ID{get; set;}
    }
    public class getDeliverList
    {
        public string mbrand { get; set; }
        public string token{get; set;}
    }
    public class getDiscount
    {
        public string mbrand { get; set; }
        public string discountCode{get; set;} public string token{get; set;} public string price{get; set;}
    }
    public class getListOfFeaturesCombinWithValue
    {
        public string mbrand { get; set; }
        public string productID{get; set;}
    }
    public class getproductdetailForCookie
    {
        public string mbrand { get; set; }
        public string idlist{get; set;}
    }
    public class getSubcatData
    {
        public string mbrand { get; set; }
        public string id{get; set;} public string token{get; set;}
    }
    public class getTime
    {
        public string mbrand { get; set; }
        public string storeID{get; set;} public string token{get; set;}
    }
    public class getTypeList
    {
        public string mbrand { get; set; }
        public string catID{get; set;} public string catLevel{get; set;}
    }
    public class isInArea
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string latitude{get; set;} public string longitude { get; set; }
    }
    public class removeAddress
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string id{get; set;}
    }
    public class sendCodeAgain
    {
        public string mbrand { get; set; }
        public string mobile{get; set;}
    }
    public class sendSMS
    {
        public string mbrand { get; set; }
        public string mobile{get; set;}
    }
    public class setAddress
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string address{get; set;} public string lat{get; set;} public string lng{get; set;} public string postalCode
           {get; set;} public string title{get; set;} public string city{get; set;} public string state{get; set;} public string id{get; set;}
    }
    public class setAUTcode
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string timestamp{get; set;} public string auth{get; set;}
    }
    public class setauth
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string timestamp{get; set;} public string auth{get; set;}
    }
    public class setWalletAuth
    {
        public string mbrand { get; set; }
        public string timestamp{get; set;} public string auth{get; set;}
    }
    public class update
    {
        public string mbrand { get; set; }
        public string model{get; set;} public string osVersion{get; set;} public string minSdk{get; set;} public string versionCode{get; set;} public string versionName{get; set;} public string os
           {get; set;} public string mobile{get; set;} public string latitude{get; set;} public string longitude{get; set;} public string token{get; set;}
    }
    public class viewArticle
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string id{get; set;}
    }
    public class viewProduct
    {
        public string mbrand { get; set; }
        public string token{get; set;} public string id{get; set;}
    }
   
}