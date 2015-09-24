<?php
ob_start();
// if(isset($_GET['id']) && $_GET['mode'] == 'edit'){
// $title = "We are editing: " . $_GET['name'] . " are you sure!!!";
// }

/*phpinfo();*/
?>
<html lang="en">
  <head>
    <meta charset="ISO-8859-1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="shortcut icon" href="/favicon.png">
    <script type="text/javascript" src="/jquery.js"></script>

    <title>A Touch of Good News</title>

    <!-- Bootstrap core CSS -->
    <link href="/bootstrap.css" rel="stylesheet">

    <!-- Custom styles for this template -->
    <link href="/starter-template.css" rel="stylesheet">

    <!-- Just for debugging purposes. Don't actually copy this line! -->
    <!--[if lt IE 9]><script src="../../docs-assets/js/ie8-responsive-file-warning.js"></script><![endif]-->
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
    <![endif]-->
  </head>

  <body>      
    <button onclick="about()" style="position:fixed; right:167px" type="button" class="btn btn-default">About</button>
    <div id="about" style="display: none; height:100%; width:100%; position:fixed;z-index:1000;background: rgba(0,0,0,0.8);">      
      <div style="color: #000; font-size: 1.2em;z-index:1001;position: fixed;width:440px;height:300px;top:50%;left:50%;padding:10px;margin:-150px 0 0 -220px; background-color:#888;border-radius:10px;">
        A Touch of Good News is an experiment in sentiment analysis. Through a combination of several algorithms, news
        articles are drawn from a few sources and then ranked based on a number of criteria. <br>
        This website attempts to automatically aggregate good news and rank the positivity of the news. 
        The order of display is based on both this ranking and the recency of the article. <br>
        improvements to the algorithm and more news sources coming soon.<br>
        <button onclick="closeAbout()" style="position: fixed; width:65px; left:50%; margin-left:-50px;" type="button" class="btn btn-default">Close</button>
      </div>
    </div>
    <script type="text/javascript">
      function about(){
          $("#about").fadeIn();
      }
      function closeAbout(){
        $("#about").fadeOut();
      }

      function anim(link) {
          var w = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
          var h = window.innerHeight || html.clientHeight  || body.clientHeight  || screen.availHeight;
          $("p").animate({height:'toggle'});
          $("h1 a").animate({fontSize:'10px'});
          $(".container").animate({width:'300px',left:w-300,marginRight:0});
          //$(".navbar").animate({height:'toggle'}); 
          $("#frame").show();
          $("#frame").animate({height:h,width:w-300});
          $('iframe').attr("src",link);
          $('#leftArr').css('display','block');
          $('h1 a').each(function(i){
            $(this).attr('href','javascript:navFrame("'+$(this).attr('rel')+'");');
          });
        }
        function animBack() {          
          $(".container").css('width','');
          $(".container").css('left','');
          $(".container").css('marginRight','');
          //$(".navbar").animate({height:'toggle'}); 
          $('#frame').css('display','none');
          $('#leftArr').css('display','none');
          $('iframe').attr("src","");
          $("p").animate({height:'toggle'});
          $("h1 a").animate({fontSize:'26px'});
          $('a').each(function(i){
            $(this).attr('href',$(this).attr('rel'));
          });
        }
        function navFrame(link){
          $('iframe').attr('src',link);
        }
    </script>
    <div id='frame'  height="0px" style="position:fixed; left:0; right:0; display:none;">
      <iframe src="" style="height:100%;width:100%" frameborder="0"></iframe>
    </div>
    <img id='leftArr' onclick="animBack();" src="/leftArrow.png" height="21px" style="position: fixed; display:none;top:5;right:5;"/>
    <!--<div class="navbar navbar-inverse navbar-fixed-top" role="navigation">
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#">A Touch of Good News</a>
        </div>
        <div class="collapse navbar-collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="#">Home</a></li>
            <!--<li><a href="#about">About</a></li>
            <li><a href="#contact">Contact</a></li> -->
          <!--</ul>
        </div><!--/.nav-collapse -->
      <!--</div>
    </div>-->
  <a href="http://www.arvixe.com" style="position:fixed; right:50px; top:6px;" target="_blank">Hosted By Arvixe</a>

    <div id="mainCont" class="container">

      <div class="news">
        <br>
        <?php
          function custSort($a,$b)
          {     
            $difA=strtotime($a['date'])-strtotime(date('j-n-Y'));
            $difB=strtotime($b['date'])-strtotime(date('j-n-Y'));
            $posA=(float)$a['pos'];
            $posB=(float)$b['pos'];                        
            if ($difA/($posA+1.1)==$difB/($posB+1.1)) return 0;
            return ($difA/($posA+1.1)>$difB/($posB+1.1))?-1:1;
          }

          $con=mysqli_connect("localhost","username","password","goodnews"); 
          if (mysqli_connect_errno()) {
            echo "Failed to connect to MySQL: " . mysqli_connect_error();
          }      
    $query = "SELECT * FROM Articles ORDER BY Date DESC LIMIT 0 , 30";
          $result=mysqli_query($con,$query);
          $news = array();
          //$count=0;
          while($row = mysqli_fetch_array($result)) {            
            $title=str_replace('(VIDEO)', '',$row['Title']);                        
            $title=str_replace('(VINE)', '',$title);  
            $news[]=Array('title'=>$title,'cat'=>$row['Category'],'link'=>$row['Link'],'date'=>$row['Date'],'source'=>$row['Source'],'pos'=>$row['Positive']);
            //$count+=1;
          }
          usort($news, "custSort");
          foreach ($news as $value) {            
            echo "<h1><a href=\"" . $value['link'] . "\" rel=\"".$value['link']."\">".$value['title']."</a></h1>";
            $date=date('h:i a, l, F j, Y', strtotime($value['date']));
            echo "<p class=\"lead\"> <img onclick=\"javascript:anim('".$value['link']."');\" height=\"21px\" src=\"/rightArrow.png\"/> " . $value['source'] . ", ".$date."</p>";            
            echo "<p class=\"lead right\">".$value['cat']."</p>";
          }
        ?>
        <!--<h1><a href="google.com">SAG Awards 2014: Analysis: Winners should keep those speeches handy</a></h1>
        <p class="lead">The LA Times, timestamp</p>
        <h1><a href="google.com">This is some news</a></h1>
        <p class="lead">The NY Times, timestamp</p>-->
      </div>

    </div><!-- /.container -->


    <!-- Bootstrap core JavaScript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="https://code.jquery.com/jquery-1.10.2.min.js"></script>
    <!--<script src="../../dist/js/bootstrap.min.js"></script>-->

<script>
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-51159969-1', 'touchofgoodnews.com');
  ga('send', 'pageview');

</script>
  </body>
</html> 
<?php 

/*ob_flush();*/?>