(function ($) {
    Array.prototype.indexOf = function(val) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == val) return i;
        }
        return -1;
    };
    Array.prototype.remove = function(val) {
        var index = this.indexOf(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };
    $.extend(true,$,{
        simpleBox : function(text, ms) {
            ms = ms || 2000;
            var jq =$('.simple_block');
            jq.html(text).show().animate({
                opacity: 1
            }, 300);
            setTimeout(function() {
                jq.animate({
                    opacity: 0
                }, 300,function(){
                    jq.hide();
                });
            }, ms);

        },
        dialogBox : function(text,title,cbA,cbB){
            var jq =$('.m_block'),jt=jq.find('.m_content'),
            m_cl = jq.find('a.m_cancel'),str='',
            m_ok=jq.find('a.m_ok'),
            m_ok_2=jq.find('a.m_ok_2');
            var closeBox = function(){
                m_cl.off();
                m_ok_2.off();
                m_ok.off();
                jq.hide();
                $('.f_mask').hide();
                //                $('body,html').css('overflow','');
                !!cbB&&cbB();
            }
            if(!cbA){
                m_ok_2.show().siblings('a').hide();
            }else{
                m_ok_2.hide().siblings('a').show();
            }
            //            $('body,html').css('overflow','hidden');
            if(title && title.length>0){
                str +='<p>'+title+'</p>'
            }
            jt.empty().append(str+text);
            jq.show();
            $('.f_mask').show();
            m_cl.on('click',closeBox);
            m_ok_2.on('click',closeBox);
            m_ok.on('click',function(){
                closeBox();
                !!cbA&&cbA();
            });

        },
        ajaxOpt:function(method,param,callback,opt){
            var med = {
                'append_prds':'add_product',
                'get_color_size':'get_color_size',
                'delete_items':'remove_items',
                'update_gift':'add_gift',
                'delete_gift':'remove_gift',
                'update_number':'cart_update_quantity', //更新购买数量
                'add_to_wishlist': 'add_to_wishlist'
            }
            param=$.extend({
                sid:sid,
                action:med[method]
            },param);
            $('.loading').show();
            $.ajax({
                url: 'cart.php',
                type: 'GET',
                data: param,
                dataType: 'json',
                success: function (result) {
                    $('.loading').hide();
                    if(result.errorCode==0){
                        !!callback&&callback(result,opt);
                        
                        setTimeout(function(){
                           method !='get_color_size' && location.reload();
                        }, 300);
                    }else{
                        $.simpleBox(result.errorMsg);
                    }
                },
                error: function (xhr, type) {
                    //                         alert('数据请求失败!');
                    $('.loading').hide();
                }
            });
        },
        isEmpty:function(obj){
            if(obj == undefined || obj == null || obj == ""){
                return true;
            }else{
                return false;
            }
        }
    });
    $.fn.initBlockSize=function(val){
        var total=0,h=$(window).height(),
        top =$(this).find('p').eq(0).height()||0,
        foot=$(this).find('.f_foot').height()||0,
        con = $(this).find('.f_content');
        if(con[0]){
            if(!val){
                total = $(this).height();
            }else{
                if(val.indexOf('rem')>0){
                    total = parseFloat(val)*20;
                }
                if(val.indexOf('px')>0){
                    total = parseFloat(val);
                }
                if(val.indexOf('%')>0){
                    total = parseFloat(val)*h/100;
                }

            }

            con.height(total-top-foot+'px');
        }
    };
    $.fn.slideUp = function(value){
        var val = value || '80%',
        d=parseInt($(this).attr('direction')),param ={};
        $('.f_mask').show();
        if(d===0){
            param.height = val;
        }else{
            param.width = val;
            val = '100%';
        }
        $(this).initBlockSize(val);
        $(this).animate(param,300);
    };
    $.fn.slideDown = function(flag){
        var d=parseInt($(this).attr('direction')),param ={};
        if(d===0){
            param.height=0;
        }else{
            param.width=0;
        }
        $(this).animate(param,200,function(){
           !flag && $('.f_mask').hide();
        });
    };
    $.fn.initSelectList=function(data,opt){
        var outer =$(this),jq =outer.find('.select_box'),
        ulist = outer.find('ul.sel_list'),
        over = outer.find('a.c_btn'),
        cancel = outer.find('a.c_close');
        var selectView ={
            init:function(){
                this.c_val=[];
                this.c_val.push(opt.pid);
                this.jsonData= this.parseData();
                this.render();
                jq.on('click','div.select_top',this.openList);
                jq.on('click','span',this.selectItem);
                ulist.on('click','a.f_del',this.delItem);
                over.on('click',this.sumbitColorSize);
                cancel.on('click',this.closeBox);
                $('.f_mask').on('click',this.closeBox);
            },
            closeBox:function(){
                outer.slideDown();
                ulist.find('li').each(function(i){
                    if(i>0){
                        $(this).empty().remove();
                    }
                });
                jq.off();
                ulist.off();
                over.off();
                $('.f_mask').off();
            },
            sumbitColorSize: function() {
                selectView.closeBox();
                var arr = selectView.c_val;
                if(arr.indexOf(opt.pid)<0){
                    var param1={};
                    param1.sid=sid;
                    param1.action ='remove_items';
                    param1.batch_ids = !!opt.batch_id?opt.batch_id:'';
                    param1.item_ids = !!opt.batch_id?'':opt.itemId;
                    $('.loading').show();
                    $.ajax({
                        url: 'cart.php',
                        type: 'GET',
                        data: param1,
                        dataType: 'json',
                        success: function (result) {
                            var param2={};
                            for(var i=0,l=arr.length;i<l;i++){
                                arr[i]= arr[i]+'.1'
                            }
                            param2.product_ids=arr.join(',');
                            $.ajaxOpt('append_prds',param2);
                        }
                    });
                }else{
                    arr.remove(opt.pid);
                    if(arr.length>0){
                        var param2={};
                        for(var i=0,l=arr.length;i<l;i++){
                            arr[i]= arr[i]+'.1'
                        }
                        param2.product_ids=arr.join(',');
                        $.ajaxOpt('append_prds',param2);
                    }
                }

            },
            openList:function(){
                var tar = $(this).next('div');
                if(tar[0].style.display!='block'){
                    tar[0].style.display='block';
                }else{
                    tar[0].style.display='none';
                }
            },
            showDelBtn:function(){
                var lis = ulist.find('li');
                if(lis.length>1){
                    $('a.f_del').css({"display":"inline"});
                }else{
                    $('a.f_del').css({"display":"none"});
                }
            },
            delItem:function(){
                var l =$(this).closest('li');
                l.empty().remove();
                selectView.c_val.remove(l.attr('item_id'));
                selectView.showDelBtn();
            },
            selectItem:function(){//分色分码选择颜色
                var cont=$(this).closest('div.f_select'),
                nc = outer.find('.sel_attrs').eq(-1),
                tar = cont.find('p'),
                val = $(this).text(),
                itemId = $(this).attr('item_id')||null,
                index =cont.index(),total = data.attr_count,
                a_name='attr'+(index+1)+'_name';
                tar.text(data[a_name]);
                if(index==1){
                    if(!itemId){
                        $.simpleBox('请按顺序选择');
                        $(this).closest('div').hide();
                        return;
                    }else{
                        nc.find('span').each(function(){
                            $(this).attr('item_id','');
                        });
                    }
                }else if(index != total-1){
                    var str='',list = selectView.jsonData[val];
                    for(var i=0,l=list.length;i<l;i++){
                        str += '<span item_id="'+list[i].itemid+'">'+list[i].size+'</span>';
                    }
                    nc.empty().append(str);
                }

                if(itemId && selectView.c_val.indexOf(itemId)<0){
                    var li = ulist.find('li').eq(0);
                    ulist.append(li.clone());
                    selectView.c_val.push(itemId);
                    selectView.showDelBtn();
                    selectView.setValue(itemId);
                }
                $(this).closest('div').hide();
            },
            setAttrs:function(){
                if(data.attr_count==1){
                    var map = selectView.jsonData,
                    ems=outer.find('.sel_attrs').eq(0).find('span');
                    ems.each(function(){
                        var k = $(this).text();
                        var that = $(this);
                        if($.isEmpty(map[k])){
                            for(var i = 0; i < map[""].length;i++){
                                if(k == map[""][i].color){
                                    that.attr('item_id',map[""][i].itemid);
                                }
                            }
                        }else{
                            that.attr('item_id',map[k][0].itemid);
                        }
                        
                        
                    });
                }
            },
            setValue:function(pid){
                var prds =data.product,l=prds.length,tit= '已选：';
                for(var i=0;i<l;i++){
                    if(pid == prds[i].itemid){
                        var dom =ulist.find('li').eq(-1);
                        dom.attr('item_id',pid);
                        dom.find('img').attr('src',prds[i].image);
                        dom.find('p.prd_price').text('￥'+prds[i].salePrice);
                        if(prds[i].color && prds[i].size){
                            tit += prds[i].color+' '+prds[i].size;
                        }if(!prds[i].size || !prds[i].color){
                                tit += prds[i].size||prds[i].color;
                            }
                        dom.find('p.prd_tit').text(tit);
                    }
                }

            },
            parseData:function(){
                var prds =data.product,l=prds.length,json={};
                var key = data.attr1_name =='颜色'?'color':'size';
                for(var i=0;i<l;i++){
                    var val =prds[i][key];
                    if(!json[val]){
                        json[val]=[];
                    }
                    json[val].push(prds[i]);

                }
                return json;
            },
            render:function(){
                var arr=[];
                for(var i=0;i<data.attr_count;i++){
                    var k ='attr'+(i+1);
                    arr .push('<div class="f_select"><div class="select_top"><p>',
                        opt.attrs[i],'</p><a class="fr select_arrow"></a></div><div class="sel_attrs">',
                        '</div></div>');
                    for(var m=0,n=data[k].length;m<n;m++){
                        arr.splice(-1,0,'<span>'+data[k][m]+'</span>');
                    }
                }
                jq.empty().append(arr.join(''));
                selectView.setAttrs();
                selectView.setValue(opt.pid);

            }

        }
        selectView.init();
    },

    $.fn.keyBoard = function (val,callback){
        var divs = $(this).find('.jp_show').find('div'),that = this,
        c_btn=$(this).find('a.c_close'),o_btn = $(this).find('a.c_ok'),
        monitor = divs.eq(1).find('input'),l_btn = divs.eq(0),r_btn=divs.eq(2),
        bg=$(that).not('a');
        var virtualKeyboard = {
            init:function(){
                monitor.val(val).focus();
                this.over=0;
                this.canScroll=0;
                this.scrollEvent();
                $('footer.total_result').hide();
                bg.on('click',this.getFocus);
                l_btn.on('click',this.lessNumber);
                r_btn.on('click',this.addNumber);
                c_btn.on('click',this.cancelPnael);
                o_btn.on('click',this.overPanel);
                monitor.on('blur',this.overFocus);
            },
            scrollEvent:function() {
            document.addEventListener('touchmove',function(e){
                if(virtualKeyboard.canScroll===0){
                    e.preventDefault();
                    e.stopPropagation();
                }
            },false);
            },
            getFocus:function () {
                monitor.focus();
            },
            overFocus:function () {
                setTimeout(function(){
                    var name =document.activeElement.tagName;
                    if(name.toLowerCase()!=='input' && virtualKeyboard.over===0){
                        virtualKeyboard.cancelPnael();
                    }

                },300);
            },
            cancelPnael:function () {
                var flag =virtualKeyboard.getValue()===0?true:false;
                $(that).slideDown(flag);
                /*$(".navbar").css({"position":"fixed"});*/
                virtualKeyboard.over=1;
                virtualKeyboard.canScroll=1;
                $('footer.total_result').show();
                bg.off();
                l_btn.off();
                r_btn.off();
                c_btn.off();
                o_btn.off();
                monitor.off();
            },
            overPanel:function () {
                var num =virtualKeyboard.getValue();
                virtualKeyboard.cancelPnael();
                if(isNaN(num)){
                    return false;
                }
                callback(num);

            },
            lessNumber:function () {
                var num =virtualKeyboard.getValue();
                if(num>0){
                    num--;
                    monitor.val(num);
                }
                return true;
            },
            addNumber:function () {
                var num =virtualKeyboard.getValue();
                num++;
                monitor.val(num);
            },
            getValue: function () {
                return parseInt(monitor.val());
            }

        }
        virtualKeyboard.init();
    }




    $(function() {
        FastClick.attach(document.body);
    });

})(Zepto);