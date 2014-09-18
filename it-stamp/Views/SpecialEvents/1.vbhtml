@Code
    ViewBag.Title = "IT勉強会スタンプラリー"
End Code

@Html.Partial("_TopBanner")

<div class="row">

    <div class="col-md-9">

        <h1>@ViewBag.Title</h1>

        <h2 id="about">開催概要</h2>
        <div style="margin-bottom: 200px;">
            <p>IT勉強会スタンプラリーは、IT勉強会に参加してスタンプを集める無料のイベントです。</p>

            <h3>IT 勉強会に参加すると楽しい！</h3>


            <p>
                毎日各所でIT勉強会が開催されています。まだ参加したことのない人はぜひ参加してみてください。<br />
                IT勉強会探しには <a href="">IT勉強会カレンダー</a> が便利です。
            </p>

            <h3>スタンプを集めると楽しい！</h3>

            <p>対象のIT勉強会 に参加するとスタンプがもらえます。また、集めたスタンプの数によって参加記念品がもらえます。</p>

            <h3>IT勉強会スタンプラリーとは……</h3>

            <ul>
                <li>IT勉強会に参加してスタンプ（またはシール）を集めるイベントです。</li>
                <li>全国で 2014月10月 から 2015年3月 まで開催します（予定）。</li>
                <li>対象の IT 勉強会 に参加すると台紙にスタンプを押してもらえます。</li>
                <li>スタンプを集めると参加記念品がもらえます。</li>
                <li>誰でも参加でき、参加は無料です。</li>
            </ul>
        </div>


        <h2 id="rule">参加してみよう！</h2>
        <div style="margin-bottom: 200px;">

            <h3>参加方法</h3>
            <ol>
                <li>スタンプラリーを実施しているIT勉強会に参加します。</li>
                <li>はじめて参加した場合、台紙をもらいます。</li>
                <li>スタンプを押してもらいます。</li>
                <li>2回目の参加から台紙を持参してください。</li>
            </ol>

            <h3>スタンプラリーのルール</h3>

            <ul>
                <li>対象のIT勉強会に参加すると1個スタンプがもらえます。</li>
                <li>参加時およびIT勉強会スタンプラリー期間終了時に参加記念品がもらえます。</li>
                <li>すべてのスタンプを集める必要はありません。</li>
            </ul>

            <h3>参加記念品</h3>
            <p>1個集めると（1回参加すると）、ステッカーをプレゼント！<br />ステッカーも台紙も数種類あるので、いろいろなIT勉強会に参加してみてください。</p>
            <p>2個以上は、クリアフォルダーを予定しています（郵送で交換）。</p>
            <h3>記念品の交換方法</h3>
            <p>記念品の交換は、台紙を郵送していただきます。記念品と併せて返送します。</p>
            <p>詳細は、このサイトでお知らせします。</p>
        </div>



        <h2 id="app">参加コミュニティ募集中！</h2>
        <div style="margin-bottom: 200px;">

            <p>IT勉強会スタンプラリーに参加いただけるコミュニティを募集中です。</p>
            <p>参加コミュニティには、スタンプラリーの台紙を送ります。また、スポンサーのノベルティのプレゼントもあります。</p>

            <h3>コミュニティの参加条件</h3>
            <p>
                参加は無料ですが、スタンプもしくはシール（オリジナル・既製品問わず）はご用意ください。
            </p>
            <p>
                参加した場合、スタンプラリーの運営に関わる作業（IT勉強会の登録および報告、スタンプの押印など）を実施していただくことになります。
                詳細は、下記の資料でご確認ください。
            </p>

            <div style="margin:50px 0 50px;">
                <div style="width: 510px" id="__ss_12092582">
                    <strong style="display: block; margin: 12px 0 4px">
                        <a href="http://www.slideshare.net/it-stamp/it-321"
                           title="IT 勉強会 スタンプラリー コミュニティ向け" target="_blank">IT 勉強会 スタンプラリー コミュニティ向け</a>
                    </strong>
                    <iframe src="https://www.slideshare.net/slideshow/embed_code/12092582?rel=0" width="510"
                            height="426" frameborder="0" marginwidth="0" marginheight="0" scrolling="no"></iframe>
                    <div style="padding: 5px 0 12px">
                        View more <a href="http://www.slideshare.net/" target="_blank">presentations</a>
                        from <a href="http://www.slideshare.net/it-stamp" target="_blank">IT 勉強会スタンプラリー</a>
                    </div>
                </div>
                <p>※ 資料内の<a href="http://it-stamp.jp/resources/manual.pdf">マニュアル</a></p>
            </div>

            <p>
                参加希望のコミュニティは、資料を確認いただいた上で <a href="">運営事務局</a> までお知らせください。
                参加検討されているコミュニティからの質問などもお待ちしています。
            </p>
        </div>

        <h2 id="sponsors">スポンサー紹介</h2>
        <div style="margin-bottom: 200px;">

            <p>IT勉強会スタンプラリーは、以下の企業・団体にご協賛いただいています。</p>

            <h4><img src="@Href("~/images/stamprally/platinum.png")" alt="" class="img-responsive" /></h4>

            <a href="#"><img src="@Href("~/images/stamprally/logo-microsoft.png")" srcset="@Href("~/images/stamprally/logo-microsoft2x.png") 2x" alt="ConoHa" class="img-responsive" style="max-width: 250px;margin:20px 0 40px;" /></a>

            <a href="#"><img src="@Href("~/images/stamprally/logo-conoha.png")" srcset="@Href("~/images/stamprally/logo-conoha2x.png") 2x" alt="ConoHa" class="img-responsive" style="max-width: 230px;margin:20px 0 40px;" /></a>

            <h4><img src="@Href("~/images/stamprally/gold.png")" alt="" class="img-responsive" /></h4>

            <a href="#"><img src="@Href("~/images/stamprally/logo-cybozulive.png")" srcset="@Href("~/images/stamprally/logo-cybozulive2x.png") 2x" alt="ConoHa" class="img-responsive" style="max-width: 180px;margin:20px 0 40px;" /></a>


            協賛金は、参加記念品代など運営諸費用に割り当てます。
        </div>


        <h2 id="qa">Q &amp; A</h2>
        <div style="margin-bottom: 200px;">

            <h3>一般参加者向け</h3>
            <ul class="list-unstyled special-event-qa">
                <li>
                    <i class="glyphicon glyphicon-check"></i> IT勉強会スタンプラリーって何？
                    <ul>
                        <li>IT勉強会に参加してスタンプを集めるイベントです。対象のIT勉強会に参加すると台紙にスタンプを押してもらえます。スタンプを集めると参加記念品がもらえます。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 参加は無料？
                    <ul>
                        <li>無料です。各IT勉強会の参加費などについては各IT勉強会の案内を参照してください。IT勉強会スタンプラリーの記念品交換時に台紙を送付する場合、送料は負担してください（返送費は不要です）。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 開催期間は？
                    <ul>
                        <li>2014年10月～2015年3月（予定）です。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 参加方法は？
                    <ul>
                        <li>対象のIT勉強会に参加すると、スタンプを押す台紙がもらえます。2回目以降は台紙を持参してください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 台紙を忘れたら？
                    <ul>
                        <li>新しい台紙をもらってください。台紙のデザインが違う場合、新しい台紙をもらってコレクションしても構いません。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 台紙をなくしたら？
                    <ul>
                        <li>新しい台紙をもらってください。なくした分のスタンプを押すことはできません。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 誰でも参加できる？
                    <ul>
                        <li>できます。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 詳しいスタンプラリーのルールは？
                    <ul>
                        <li><a href="#rule" class="page-scroll">参加方法・ルール</a> を参照してください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 記念品の内容や交換方法は？
                    <ul>
                        <li><a href="#rule" class="page-scroll">参加方法・ルール</a> を参照してください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 対象のIT勉強会は？
                    <ul>
                        <li><a href="#">IT勉強会一覧</a> を参照してください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 参加しているコミュニティは？
                    <ul>
                        <li><a href="#">コミュニティ一覧</a> を参照してください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> IT勉強会の多い地域と少ない地域で何か考慮はある？（スタンプを取得する機会が異なる）
                    <ul>
                        <li>ありません。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 台紙の代わりに「IT勉強会スタンプ」のチェックインや、スマートフォンのアプリは使わないの？
                    <ul>
                        <li>今回は（も）、実際の紙で集める楽しさと誰でも参加できることを重視しました。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 誰が運営してる？
                    <ul>
                        <li>IT 勉強会を主催しているコミュニティメンバーが集まり、有志で運営しています。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> その他、わからないこと・困ったことがあったら？
                    <ul>
                        <li>運営事務局までお問い合わせください。</li>
                    </ul>
                </li>

            </ul>

            <h3>コミュニティ向け</h3>
            <ul class="list-unstyled special-event-qa">
                <li>
                    <i class="glyphicon glyphicon-check"></i> コミュニティの参加費は？
                    <ul>
                        <li>無料です。スタンプまたはシール（オリジナル・既製品問わず）は、ご用意ください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> 対象のIT勉強会は？
                    <ul>
                        <li>広く一般に募集している勉強会が対象です。内容やテーマ、IT勉強会のスタイル（セミナー形式やディスカッション形式など）は限定しません。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> コミュニティの参加条件は？
                    <ul>
                        <li>スタンプラリー運営に関わる作業を実施・協力していただけることが条件です。<a href="#app" class="page-scroll">コミュニティ参加</a> の資料をご確認ください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> コミュニティの参加方法や作業内容は？
                    <ul>
                        <li><a href="#app" class="page-scroll">コミュニティ参加</a> の資料をご確認ください。</li>
                    </ul>
                </li>
                <li>
                    <i class="glyphicon glyphicon-check"></i> その他、わからないことがあったら？
                    <ul>
                        <li>運営事務局までお問い合わせください。</li>
                    </ul>
                </li>
            </ul>
        </div>



    </div>
    <div class="col-md-3">
        <div data-spy="affix" class="list-group hidden-xs hidden-sm hidden-print affix">
            <ul class="nav nav-pills nav-stacked">
                <li><a href="#about" class="page-scroll">About</a></li>
                <li><a href="#rule" class="page-scroll">参加してみよう！</a></li>
                <li><a href="#app" class="page-scroll">参加コミュニティ募集中！</a></li>
                <li><a href="#sponsors" class="page-scroll">スポンサー紹介</a></li>
                <li><a href="#qa" class="page-scroll">Q &amp; A</a></li>
            </ul>
        </div>
    </div>
</div>


@Section Styles

End Section

@Section Scripts

    <script src="/Scripts/jquery.easing.min.js"></script>
    <script>
        (function ($) {
            var offset = 50 + 30; /* navbar + margin */

            $("body").scrollspy({
                target: ".affix",
                offset: offset
            })

            $(".affix").affix({
                offset: {
                    top: $(".bx-wrapper").outerHeight() + 60 - 20, //
                    bottom: 420 // footer + margin
                }
            }).on('.affix', function () {
                //$(this).css({
                //    'top': '0px'
                //});
                console.log('affix.bs.affix');

            }).on('affixed.bs.affix', function () {
                console.log('affixed.bs.affix');
            }).on('affix-bottom.bs.affix', function () {
                console.log('affix-bottom.bs.affix');
            }).on('affixed-bottom.bs.affix', function () {
                console.log('affixed-bottom.bs.affix');
            });

            //var a = $(".affix").affix({
            //    offset: {
            //        top: 431-70,
            //        bottom: 431
            //    }
            ////}).on('affix.bs.affix', function () {
            ////    $(this).css({
            ////        'top': '70px'
            ////    })
            //});

            //a.width(a.parent().width());

            ////jQuery to collapse the navbar on scroll
            //$(window).scroll(function () {
            //    if ($(".navbar").offset().top > 50) {
            //        $(".navbar-fixed-top").addClass("top-nav-collapse");
            //    } else {
            //        $(".navbar-fixed-top").removeClass("top-nav-collapse");
            //    }
            //});

            //jQuery for page scrolling feature - requires jQuery Easing plugin
            $('a.page-scroll').bind('click', function (event) {
                var $anchor = $(this);
                $('html, body').stop().animate({
                    scrollTop: $($anchor.attr('href')).offset().top - offset + 5
                }, 600, 'easeInOutExpo');
                event.preventDefault();
            });


        })(jQuery);
    </script>
End Section





