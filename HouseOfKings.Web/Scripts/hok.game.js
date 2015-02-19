$(function () {
    var game = $.connection.game; // the generated client-side hub proxy
    var $btn = $('.action-pick-card');
    var $groupCircle = $('.group-circle');
    var groupName = null;
    function init() {
        groupName = $('#group-name').val();
        game.server.joinGroup(groupName).done(function () {
            $(document).on('click', '.action-pick-card', function () {
                $btn.loadingButton({ text: 'Picking Card...' });
                game.server.pickCard(groupName).done(function () {
                    $btn.text('Waiting for my turn...');
                });
            });
        });
    }

    function drawGroup(groupInfo) {
        groupInfo.playerNames.forEach(function (name) {
            addPlayer(name);
        });
        drawPlayers();
        drawTurn(groupInfo.turn);
    }

    function drawPlayers() {
        var radius = 120;
        var players = $('.player');
        width = $groupCircle.width(), height = $groupCircle.height(),
        angle = 0,
        step = (2 * Math.PI) / players.length;
        players.each(function () {
            var x = Math.round(width / 2 + radius * Math.cos(angle) - $(this).width() / 2);
            var y = Math.round(height / 2 + radius * Math.sin(angle) - $(this).height() / 2);
            
            $(this).css({
                left: x + 'px',
                top: y + 'px'
            });
            angle += step;
        });
    }

    function parseSuit(suit) {
        $('.suit').removeClass('suit-heart suit-diamond suit-spade suit-club');

        switch (suit) {
            case 0: {
                $('.suit').addClass('suit-club');
                break;
            }
            case 1: {
                $('.suit').addClass('suit-diamond');
                break;
            }
            case 2: {
                $('.suit').addClass('suit-heart');
                break;
            }
            case 3:
            default: {
                $('.suit').addClass('suit-spade');
                break;
            }
        }
    }

    function parseNumber(number) {
        switch (number) {
            case 1: {
                number = 'A';
                break;
            }
            case 11: {
                number = 'J';
                break;
            } case 12: {
                number = 'Q';
                break;
            }
            case 13: {
                number = 'K';
                break;
            }
            default: {
                break;
            }
        }

        return number;
    }

    function updateStats(turn) {
        $('#card-count').html(turn.cardCount);
        $('#king-count').html(turn.kingCount);
    }

    function updateCard(card) {
        var number = null;
        parseSuit(card.suit);
        number = parseNumber(card.number);
        $('.suit').html(number);
        console.log(card);
    }

    function addPlayer(name) {
        game.client.setAudit(name + ' joined the game');
        $groupCircle.append('<div class="player">' + name + '</div>');
    }

    game.client.drawGroup = function (groupInfo) {
        drawGroup(groupInfo);
    }

    game.client.addPlayer = function (playerUsername) {
        addPlayer(playerUsername)
        drawPlayers();
    }

    game.client.setTurn = function () {
        $btn.loadingButton({ reset: true });
    }

    function drawTurn(turn) {
        updateStats(turn);

        var card = turn.card;

        if (card) {
            setAudit(turn.player + ' picked ' + '<span class="suit"></span>');
            //setMessage(turn.player + ' picked ' + '<div style="display:inline;" class="suit"></div><br/><br/><strong>' + card.rule.title + '</strong><br/><br/>' + card.rule.desc);

            updateCard(card);
        }
    }

    game.client.drawTurn = function (turn) {
        drawTurn(turn);
    }

    //game.client.showGameOver = function (card) {
    //    setAudit(card.player + ' picked ' + '<span class="suit"></span>');
    //    setMessage(card.player + ' picked ' + '<div style="display:inline;" class="suit"></div><br/><br/><strong>Game Over</strong><br/><br/>');

    //    updateStats(card);
    //    updateCard(card);
    //}

    function setAudit(message) {
        $('#audit').html(message);
    }

    game.client.setAudit = function (message) {
        setAudit(message);
    }

    function setMessage(message) {
        $('#message').html(message);
    }

    game.client.setMessage = function (message) {
        setMessage(message);
    }

    // Start the connection
    $.connection.hub.start().done(init);
});