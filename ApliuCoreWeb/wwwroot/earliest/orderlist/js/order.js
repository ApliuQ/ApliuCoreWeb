var arr=$('.list_order').attr('id').split('_');
var sid=arr[1];
var condition_time=arr[2];
var condition_status=arr[3];
var keyword=arr[4]; 
var page_count=arr[5];
var popTop=arr[6];
var initTop = arr[6];
var action=window.location.search.split('=')[1].split('&')[0];
if($('.has_topay').attr('id')=='haspay'){//有合并支付按钮
    var payCenter=$(".topay").attr("id").slice(0,$(".topay").attr("id").lastIndexOf("="))+"=";
    var payLink="";
}
var subReasonContent='';

var order = {
    init: function () {  
        //选择列表页面弹层
        $('.prd_order').on('click', this.toggle_select);
        $('#prd_ck,#head_mask').on('click', this.close_select);
        //工具弹层
        $('.order_tool').on('click', this.toggle_tool);
        $('#search_mask,#go_search').on('click', this.close_tool);
        //搜索弹层
        $('.order_search').on('click', this.pop_search);
        $('.del_search').on('click', this.del_search);       
        //筛选弹层
        $('.order_filter').on('click', this.pop_filter);
        $('.close_fil,.go_filter').on('click', this.close_filter);
        $('.fil1 span').on('click', this.filter_time);
        $('.fil2 span').on('click', this.filter_stat);
        //返回顶部
        $('#backtop').on('click', this.back_top);
        //提示弹窗
        $('.m_ok,.save_rec,.save_invoice').on('click', this.ok_tips);
        $('.m_no').on('click', this.close_tips);
        //修改收货人信息
        $('.change_recerver_line').on('click', this.pop_recerver);
        $('.close_rec').on('click', this.close_recerver);
        //修改发票信息
        $('.change_invoice_line').on('click', this.pop_invoice);
        $('.del_input_inv').on('click', this.del_invoice);
        $('.close_invoice').on('click', this.close_invoice);
        //合并支付
        this.initCheck();
        $(".total_result .c_checkbox").on("click",this.checkAll);                 
        $(".content_order").on("click",".shop_title .c_checkbox",this.checkOrder);
        $(".total_result .topay").on("click",this.topay);  
        //取消订单理由
        $(".ritem").on("click",this.reasonChose); 
        $(".otherReason").on("click",this.otherChose);
        $(".tell").on("click",this.otherCheck);
        $(".submit_c_reason").on("click",this.subReason);
    },
    toggle_select:function(){
        if(!$('.prd_order').hasClass('no_click'))
        { 
            popTop=initTop;
            $('.prd_order').toggleClass("trans_arrow");
            $('#head_mask,.o_select,.order_tool').fadeToggle();
            $('#head_mask').css('opacity','0.4');
            ctrl_scroll();                
        }
    },
    close_select:function(){
        $("#head_mask,.o_select").css("display","none");
        $('.order_tool').fadeToggle();          
        $('.prd_order').removeClass("trans_arrow");        
        ctrl_scroll();           
        window.scrollTo(0, popTop);
    },
    toggle_tool:function(){
        popTop=initTop;
        $('.toolbar,#search_mask').fadeToggle();       
        $('#search_mask').css('opacity','0.4');
        $('.prd_order').toggleClass("no_click"); 
        ctrl_scroll();                         
    },
    close_tool:function(){
        $("#search_mask,.searchbar,.toolbar").css("display","none");
        $('.prd_order').removeClass("no_click");
        ctrl_scroll(); 
        window.scrollTo(0, popTop);
    },
    pop_search:function(){
        $("#search_mask,.searchbar").css("display","block");            
        $(".toolbar").css("display","none");
        $("#search_text").focus();         
        $('.prd_order').removeClass("no_click");
    },
    del_search:function(){
        $("#search_text").val("");
    },
    pop_filter:function(){
        $(".ol_filter,#pop_mask").css("display","block");    
        $("#search_mask,.toolbar,.order_tool").css("display","none");          
        var ol_height=document.body.clientHeight-$(".ol_filter").height();
        $(".ol_filter").animate({
            top:ol_height
        },"2000"); 
        $(".fil_content2").fadeToggle('3000');
        $('.fil_content span').removeClass('orange_hollow');
        $('#time_1').addClass('orange_hollow');               
    },
    close_filter:function(){
        ctrl_scroll();
        window.scrollTo(0, popTop);
        $(".ol_filter").animate({
            top:'100%'
        },"3000");
        $(".ol_filter,#pop_mask").css("display","none");
        $(".fil_content2").fadeToggle('3000');
        $('.order_tool').fadeToggle();    
        $('.prd_order').removeClass("no_click");
    },
    filter_time:function(){
        $('.fil1 span').removeClass('orange_hollow');    
        $(this).toggleClass('orange_hollow');
        $('#filter_time').val($(this).attr('id').split('_')[1]);
    },
    filter_stat:function(){
        $('.fil2 span').removeClass('orange_hollow');    
        $(this).toggleClass('orange_hollow');
        $('#filter_stat').val($(this).attr('id').split('_')[1]);
    },
    pop_recerver:function(){      
        $(".ol_rec").animate({
            left:'0%'
        },"3000",function(){
           $(".content_ol_detail").hide();
           $(".ol_detail_action").hide();
        });
    },
    close_recerver:function(){ 
        $(".content_ol_detail").show();
        $(".ol_detail_action").show();        
        $(".ol_rec").animate({
            left:'100%'
        },"3000");
    },
    pop_invoice:function(){
        $(".ol_invoice").animate({
            left:'0%'
        },"3000"); 
    },
    del_invoice:function(){         
        $("#invoice_title").val("");
    },
    close_invoice:function(){         
        $(".ol_invoice").animate({
            left:'100%'
        },"3000");
    },
    initCheck:function(){
        countPrice();
        $(".r_checkbox").prop("checked",false);
        $('.tell').val('');
    },    
    checkAll:function(){
        var c = $(this).prop("checked");        
        if(!c){
            $(".shop_title .c_checkbox").prop("checked",false);
        }else{
            $(".shop_title .c_checkbox").prop("checked",true);
        }
        countPrice();
    },
    checkOrder:function(){        
        var c = $(this).prop("checked");
        var totalNum = $(".shop_title .c_checkbox").length;
        var orderNum = parseInt($(".total_result .orderNum").html());
        if(!c){
            $(".total_result .c_checkbox").prop("checked",false);
        }else{
            if(orderNum + 1 == totalNum){
                $(".total_result .c_checkbox").prop("checked",true);
            }
        }
        countPrice();
    },
    topay:function(){
        var orderNum = parseInt($(".total_result .orderNum").html());
        tipMsgs = "请选择至少一个订单";
        if(orderNum <= 0){
            /*new Toast({
                context:$('body'),
                message:tipMsgs,
                time:2000
            }).show();*/
            $.simpleBox(tipMsgs);
        }else{
            window.location.href=payLink;
        }
    },
    ok_tips:function(){
        var arr=$(this).attr('id').split('_');
        var popType=arr[0];
        var orderid=arr[1];
        var sid=arr[2];
        var rec_name='';
        var rec_adr='';
        var rec_tel='';
        var rec_mob='';
        var inv_tit='';
        var inv_con='';
        var action_ajax='';
        switch(popType)
        {
            case 'del':
                action_ajax='delete_order';
                msg='订单删除';
                break;
            case 'cel':
                action_ajax='cancel_order';
                msg='订单取消';
                break;
            case 'cep':
                action_ajax='express_submit';
                msg='确认收货';
                break;
            case 'sr':
                action_ajax='update_invoice_receiver';
                msg='修改';
                rec_name=$("#rec_name").val();
                rec_adr=$("#rec_adr").val();
                rec_tel=$("#rec_tel").val();
                rec_mob=$("#rec_mob").val();
                break;
            case 'si':
                action_ajax='update_invoice_receiver';
                msg='修改';
                inv_tit=$("#invoice_title").val();
                inv_con=$("#inv_con").val();
                break;
        }        
        $("#pop_mask,.m_tips").css("display","none");        
        $("html").removeClass('no_sro');
        document.body.style.overflow='scroll';
        window.scrollTo(0, popTop);  
        

        $.ajax({
            type: 'post',
            url: 'order.php?sid='+sid,
            data: {
                action:action_ajax,
                orderid:orderid,
                rec_name:rec_name,
                rec_adr:rec_adr,
                rec_tel:rec_tel,
                rec_mob:rec_mob,
                inv_tit:inv_tit,
                inv_con:inv_con
            },                
            success: function (data) { 
                var tipMsgs=(data==='0')?'成功':'失败';
                tipMsgs=msg+tipMsgs;
                /*new Toast({
                    context:$('body'),
                    message:tipMsgs,
                    time:2000
                }).show();*/
                $.simpleBox(tipMsgs);
                if(data==='0')  
                { 
                    switch(popType)
                    {
                        case 'del':
                            if($('#ol_'+orderid).siblings().length==2) {
                                $('#ol_'+orderid).parent().parent().fadeOut(1000);
                            }
                            else {
                                $('#ol_'+orderid).fadeOut(1000);
                            }
                            break;
                        case 'cel':
                            $('#oltit_'+orderid).html('已取消');
                            $('#order_stats').html('已取消');                
                            $('#cel_'+orderid).parent().html('<a onclick="javascript:pop_tips(\'del\','+orderid+')">删除订单</a>');
                            $('.ol_detail_action').hide();
                            if(condition_status==1){
                                var len=$('.block').length;
                                if($('#ol_'+orderid).siblings().length==2) {
                                    $('#ol_'+orderid).parent().parent().remove();
                                }
                                else {
                                    $('#ol_'+orderid).remove();
                                }
                                countPrice();
                                if(len==1) {
                                    $('.content_order').html('<div class="list_null"><p>您暂无待付款订单.</p></div>');
                                    $('.total_result').hide();
                                }
                            }
                            setTimeout(function(){ 
                                console.log(action);
                                console.log("order.php?action=cancel_reason&order_id="+orderid+"&sid="+sid+"&condition_time="+condition_time+"&condition_status="+condition_status+"&keyword="+keyword+"&popTop="+initTop+"&action_last="+action);
                                window.location.href="order.php?action=cancel_reason&order_id="+orderid+"&sid="+sid+"&condition_time="+condition_time+"&condition_status="+condition_status+"&keyword="+keyword+"&popTop="+initTop+"&action_last="+action;
                            },100);                                                        
                            break;
                        case 'cep':
                            $('#oltit_'+orderid).html('已送达');
                            $('#order_stats').html('已送达');                
                            $('#cep_'+orderid).css('display','none');
                            if(condition_status==5){
                                var len=$('.block').length;
                                if($('#ol_'+orderid).siblings().length==2) {
                                    $('#ol_'+orderid).parent().parent().fadeOut(1000);
                                }
                                else {
                                    $('#ol_'+orderid).fadeOut(1000);
                                }
                                if(len==1) {
                                    $('.content_order').html('<div class="list_null"><p>您暂无收货款订单.</p></div>');                                    
                                }
                            }  
                            break;
                        case 'sr':
                            var tel=(rec_mob=='')?rec_tel:rec_mob;                           
                            if(rec_name.length>=6){
                                rec_name=rec_name.slice(0,6)+'...';
                            }
                            if(tel.length>=20){
                                tel=tel.slice(0,20)+'...';
                            }
                            $('#r_name').html('收货人：'+rec_name+'<span class="fr">'+tel+'</span>');
                            $('#r_adr').html(rec_adr);
                            break;
                        case 'si':
                            if(inv_tit.length>=15){
                                inv_tit=inv_tit.slice(0,15)+'...';
                            }
                            $('#inv_name').html(inv_tit); 
                            break;
                    }                     
                }                     
            }
        });
    },
    close_tips:function(){
        $("#pop_mask,.m_tips").css("display","none");        
        ctrl_scroll(); 
        window.scrollTo(0, popTop);
    },
    back_top:function(){
        window.scrollTo(0, 0);
        $(".fixed_box").css('display','none');
        $('.navbar,.ol_select_bar').attr("style","");        
    },
    reasonChose:function(){
        $('.tell').val('');
        subReasonContent=$(this).children('input').val();
        if(typeof(subReasonContent)=="undefined") subReasonContent='其他';        
    },
    otherChose:function(){
        $(".tell").focus();
    },
    otherCheck:function(){        
        $('.otherReason').click();
    }    
}
 
//滚轴相关 
var page=1;
var pagesro=2;
$(window).scroll(function(){
    var scrollTop = document.body.scrollTop||document.documentElement.scrollTop;         
    var viewHeight = $(window).height();
    var pageHeight = $(document).height();       
    //滑动隐藏页头页尾
    if((scrollTop > initTop)&&(scrollTop >100)){
        $('.navbar,.ol_select_bar').css('display','none');          
    } else { 
        $('.navbar,.ol_select_bar').attr("style","");        
    }
    //显示回到顶部
    if(scrollTop>1.5*viewHeight){
        $(".fixed_box").css('display','block');
    }else
    {
        $(".fixed_box").css('display','none');
    }
    //记录滚动位置
    setTimeout(function(){
        initTop = scrollTop;
    },0);
    //ajax请求翻页数据
    if((scrollTop+viewHeight)>(pageHeight-20)){            
        if((page!=pagesro)&&(pagesro<=page_count)){
            page=pagesro;                
            $(".content_order").append("<div id='loading' style='padding-top:1rem;text-align:center;'><img src='images/order/loading.gif' style='width:2rem;'/></div>");                            
            $.ajax({                            
                type: 'post',
                url: 'order.php?action=list_ajax_order&sid='+sid,
                data: {                            
                    page:page,
                    condition_time:condition_time,
                    condition_status:condition_status,
                    keyword:keyword,
                    ajax_action:action
                },                
                success: function (data) {
                    $("#loading").remove();
                    $(".content_order").append(data);
                    countPrice();
                    pagesro=page+1;                                    
                }
            });          
        }
    }        
});  
    
function ctrl_scroll(){
    $("html").toggleClass('no_sro');
    document.body.style.overflow=(document.body.style.overflow=='scroll')?'hidden':'scroll';
}

//合并支付计算数量价格变动
function countPrice(){
    var orderPrice=0;
    var orderVal='';
    var num = 0;
    for(var i=0;i<$(".shop_title .c_checkbox").length;i++){
        var c = $(".shop_title .c_checkbox").eq(i);
        if(c.prop("checked")) {
            num++;
            var val=c.val().split(';');
            for(var j=0;j<val.length-1;j=j+1){                
                var price=val[j].split(',')[1];
                orderPrice+=parseFloat(price);
            }          
            orderVal+=c.val();
        }
    }
    $(".total_result .orderNum").html(num);
    $(".total_result .orderPrice").html(orderPrice.toFixed(2));
    payLink=payCenter+orderVal;    
}

//提示弹窗
function pop_tips(popType,orderId){
    var msg='';
    $('.m_ok').html('确定');
    $('.m_no').html('返回');
    switch(popType)
    {
        case 'del':
            msg='确定删除此订单？';
            break;
        case 'cel':
            msg='取消订单后,该商品的所有优惠您都无法享受了，继续取消？';
            $('.m_ok').html('嗯呐');
            $('.m_no').html('再想想');
            break;
        case 'cep':
            msg='您确认已经收到相关商品了吗？';
            break;
    }
    popTop=initTop;
    ctrl_scroll(); 
    $(".navbar,.ol_select_bar").attr("style","");    
    $(".m_ok").attr("id",popType+"_"+orderId+"_"+sid); 
    $(".m_content span").text(msg);
    $(".m_tips,#pop_mask").css("display","block");
}

//提交搜索判断
function check_search(form) {
    var search_txt=form.search_txt.value.replace(/\s/g, "");   
    if(search_txt==''){
        /*new Toast({
            context:$('body'),
            message:'搜索关键词不能为空',
            time:2000
        }).show();*/
        $.simpleBox("搜索关键词不能为空");
        return false;
    }
    return true;
}

//进入订单详情页
function get_order(order_id){    
    window.location.href="order.php?action=get_order&order_id="+order_id+"&sid="+sid+"&condition_time="+condition_time+"&condition_status="+condition_status+"&keyword="+keyword+"&popTop="+initTop+"&action_last="+action;
}

//重新购买
function re_buy(cart_url,product_ids){ 
    $.ajax({                            
        type: 'post',
        url: 'order.php?action=cart_append_products&sid='+sid,
        data: {                            
            product_ids:product_ids                            
        },                
        success: function (data) {
            var arr=data.split('_');
            var errorCode=arr[0];
            var errorMsg=arr[1];
            var msg='';
            if(errorCode==0){
                window.location.href=cart_url+"&action=list_cart";
            }
            else{
                msg=(errorMsg==='接口异常')?'系统繁忙，请稍后重试':errorMsg.split('；')[0];
               /* new Toast({
                    context:$('body'),
                    message:msg,
                    time:3000
                }).show();*/
                $.simpleBox(msg);
            } 
        }
    });          
}

//提交取消理由
function subReason(back_url,order_id){  
    if(subReasonContent=='')
    {
        /*new Toast({
            context:$('body'),
            message:'请赐予一个理由吧',
            time:3000
        }).show();*/
        $.simpleBox("请赐予一个理由吧");
    }else{
        if((subReasonContent=='其他')&&($('.tell').val()!= "")){
            subReasonContent=$('.tell').val();
        }
        $.ajax({                            
            type: 'post',
            url: 'order.php?action=submit_reason&sid='+sid,
            data: {                            
                order_id:order_id,
                content:subReasonContent            
            },                
            success: function (data) {
                /*new Toast({
                    context:$('body'),
                    message:data,
                    time:3000
                }).show(); */ 
                $.simpleBox(data);
                setTimeout(function(){
                    window.location.href=back_url;
                },200)
            }            
        });
    }
} 

//跳物流详情页
function get_express(order_id){
    window.location.href="order.php?action=get_express&order_id="+order_id+"&sid="+sid+"&condition_time="+condition_time+"&condition_status="+condition_status+"&keyword="+keyword+"&popTop="+initTop+"&action_last="+action;
}


/** 
 * 模仿android里面的Toast效果，主要是用于在不打断程序正常执行的情况下显示提示数据 
 * @param config 
 * @return 
 */  
var Toast = function(config){  
    this.context = config.context==null?$('body'):config.context;//上下文  
    this.message = config.message;//显示内容  
    this.time = config.time==null?3000:config.time;//持续时间  
    this.left = config.left;//距容器左边的距离  
    this.top = config.top;//距容器上方的距离  
    this.init();  
}  
var msgEntity;  
Toast.prototype = {  
    //初始化显示的位置内容等  
    init : function(){  
        $("#toastMessage").remove();  
        //设置消息体  
        var msgDIV = new Array();  
        msgDIV.push('<div id="toastMessage">');  
        msgDIV.push('<span>'+this.message+'</span>');  
        msgDIV.push('</div>');  
        msgEntity = $(msgDIV.join('')).appendTo(this.context);  
        //设置消息样式  
        var left = this.left == null ? this.context.width()/2-msgEntity.find('span').width()/2 : this.left;  
        var top = this.top == null ? '50%' : this.top;  
        var height = msgEntity.height();
        var width = msgEntity.width();
        alert(height);
        msgEntity.css({
            position:'fixed',
            top:top,
            'z-index':'99999',
            left:left,
            'background-color':'black',
            color:'white',
            'font-size':'1.1rem',
            padding:'10px',            
            display:'none',
            marginTop:-height / 2
        });  
    //msgEntity.hide();  
    },  
    //显示动画  
    show :function(){  
        msgEntity.css('display','block');      
        msgEntity.fadeOut(this.time);  
    }           
} 

window.onload=function(){
    order.init();
    if(action!='get_order')  window.scrollTo(0, popTop);
}  
$.extend(true,$,{
    simpleBox:function(text, ms){
        ms = ms || 2000;
        var jq =$('.simple_block');
        jq.html(text).show().animate({
            opacity: 1,
            position:'fixed',
            top:"50%",
            'z-index':'99999',
            left:"50%",
            marginTop:-jq.height() / 2,
            marginLeft:-jq.width() / 2
        }, 300);
        setTimeout(function() {
            jq.animate({
                opacity: 0
            }, 300,function(){
                jq.hide();
            });
        }, ms);
    }
})
 