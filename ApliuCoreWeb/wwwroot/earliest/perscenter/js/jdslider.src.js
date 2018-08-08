/*
* JD 滑块 版本 1.0
*  wuhaixin@jd.com
* 1. 单元元素可自适应可视宽度
* 2. 屏幕翻转时自适应（正在优化）
* 3. 支持有序多行滑动
* 4. 支持无序单行滑动
*
*
* 待解决问题：1. 屏幕翻转自适应（主要为题是“滑动幅度”），2. 滑块末尾空白（目前没有分组的概念，所以无法动态设定滑动幅度!?）
* */

;(function($){

    $.fn.jdSlider = function (config) {

        var _wrapperWidth, _itemPerScreen;
        var _slider = $(this);
        var _divContainer = $(this).find('.jd-slider-container');
        var _sliderItem = $(this).find('.jd-slider-item');
        var _sliderWidth;
        var _sliderItemWidth = _sliderItem.eq(0).width();
        var _box = {initX: 0, initY: 0, startX: 0, endX: 0, sliderX: 0, _sliderDirection:1, startY: 0, sliderFlag: false};

        //缺省配置
        var _config = $.extend({}, {
            lineNum: 1,
            fitToScreen:false
        }, config);



        /*为了使图片单元平铺并适应可视宽度，避免一屏幕内出现不完全显示的单元*/
        var setItems = function(){

            if(!_config.fitToScreen){
               return;
            }

            _wrapperWidth = _slider.width();//获取容器可视区域宽度
            var itemW = _sliderItemWidth;
            //console.log('PaddingLeft is : '+parseInt(_sliderItem.eq(0).css('padding-left')));
            //console.log('PaddingRight is : '+parseInt(_sliderItem.eq(0).css('padding-right')));
            //console.log('MarginLeft is : '+parseInt(_sliderItem.eq(0).css('margin-left')));
            //console.log('MarginRight is : '+parseInt(_sliderItem.eq(0).css('margin-right')));

            //console.log('itemW='+itemW);


            var remainW = Number(_wrapperWidth) % itemW;
            _itemPerScreen = parseInt(_wrapperWidth / itemW);

            itemW = itemW + parseInt(remainW/_itemPerScreen);

            //设置单元宽度
            _divContainer.css({
                width:_sliderItem.length / _config.lineNum * itemW
            });
            //设置包裹单元的容器宽度
            _sliderItem.css({
                width:itemW
            });

        };


        //初始化滑块
        var initSlider = function(){
            _box.endX = 0;
            _wrapperWidth = _slider.width();//屏幕旋转时重新设定滑块可视区域宽度
            setItems();//屏幕旋转时重新设定滑块单元宽度

            /*判断行数，如果是1行，直接获取容器的总长度，如果是2行，需要按照行数乘以item总数再除以2.*/
            if(_config.lineNum==1){
                _sliderWidth = Number(_divContainer.width());
            }else{
                _sliderWidth = Number(_sliderItem.width()) * parseInt(_sliderItem.length / _config.lineNum);//获取容器可视区域宽度
            }

            //设置高度
            _divContainer.css({
                height:Number(_sliderItem.outerHeight(true))*_config.lineNum
            });

            window.addEventListener('orientationchange', function(){
                initSlider();
            });
        };

        //滑动效果
        var sliding = function(k){
            _divContainer.css("-webkit-transform", "translate3d(" + (_box.endX = k) + "px,0,0)");
        };

        _slider.bind('touchstart',function(e){
            _box.sliderFlag = true;
             var et = e.originalEvent.touches[0];
            _box.startX = et.pageX;
            _box.initX = et.pageX;
            _box.initY = et.pageY;
        });

        _slider.bind('touchmove',function(e){
             var et = e.originalEvent.touches[0];

            //console.log(Math.abs(et.pageY - _box.initY) / Math.abs(et.pageX - _box.initX));

            /*判断滑动时手指的滑动方向*/
            if (_box.sliderFlag && Math.abs(et.pageY - _box.initY) / Math.abs(et.pageX - _box.initX) < 0.6) {


                //_box.endX = et.pageX - _box.startX +  _box.endX;
                //var temEndX = et.pageX - _box.startX +  _box.endX;
                _box._sliderDirection = (et.pageX - _box.startX < 0) ? 'LEFT' : 'RIGHT';

                //_divContainer.css("-webkit-transform", "translate3d(" + temEndX + "px,0,0)");
               // _box.startX = et.pageX;
                e.preventDefault();
            }else{

                _box.sliderFlag = false;
            }
        });

        _slider.bind("touchend", function(e){
            if (!_box.sliderFlag) return;//
             var et = e.originalEvent.touches[0];

            if(_box.endX > 0){

                sliding(_box.endX = 0);
            }else if(_box.endX < -(_sliderWidth-_wrapperWidth)){
                sliding(_box.endX = -(_sliderWidth-_wrapperWidth));

            }else{

                //console.log('_wrapperWidth='+_wrapperWidth);

                if(_box._sliderDirection == 'LEFT') {
                    //console.log('LEFT');

                    //_sliderItem.length / _config.lineNum  %  _itemPerScreen
                    //翻到最后一页时，可见范围内所剩单元的宽度总和（iw）和小于可见范围的宽度。则滑动幅度为所剩单元宽度总和（iw）。
                    var limitLeftX = _sliderWidth - _wrapperWidth;
                    //console.log('limitLeftX = ' + limitLeftX);
                    _box.sliderX = (_divContainer.width() -_wrapperWidth - Math.abs(_box.sliderX) < _wrapperWidth) ? -limitLeftX : _box.sliderX - _wrapperWidth;
                    //console.log('_box.sliderX='+_box.sliderX);
                    //console.log('_divContainer.width='+_divContainer.width());
                    sliding(_box.sliderX);//往左滑动

                }else{
                    //console.log('RIGHT');
                    //_box.sliderX = (_divContainer.width() -_wrapperWidth - Math.abs(_box.sliderX) < _wrapperWidth) ? _box.sliderX - _sliderItemWidth : _box.sliderX - _wrapperWidth;


                    _box.sliderX = (_box.sliderX + _wrapperWidth > 0) ? 0 : _box.sliderX + _wrapperWidth;

                    //console.log('_box.sliderX = ' + _box.sliderX);

                    sliding(_box.sliderX);
                }

            }
            _box.sliderFlag = false;

        });

        return initSlider();

    }

})(jQuery);

